using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Utils; // CID oluşturmak için gerekli
using Rentaly.BusinessLayer.Abstract;
// System.Drawing, ImageProcessing vs. kütüphanelerini KALDIRDIK. Hatalar gitti.

namespace Rentaly.WebUI.Controllers
{
    public class RentalController : Controller
    {
        private readonly IRentalService _rentalService;
        private readonly ICustomerService _customerService;

        public RentalController(
            IRentalService rentalService,
            ICustomerService customerService)
        {
            _rentalService = rentalService;
            _customerService = customerService;
        }

        // ── Rental Listesi (Burası Değişmedi) ──
        public async Task<IActionResult> RentalList(string status = "")
        {
            var all = await _rentalService.TGetAllRentalsWithDetailsAsync();

            ViewBag.TotalCount = all.Count;
            ViewBag.BeklemdeCount = all.Count(x => x.Status == "Beklemede");
            ViewBag.OnaylananCount = all.Count(x => x.Status == "Onaylandı");
            ViewBag.IptalCount = all.Count(x => x.Status == "İptal");
            ViewBag.ToplamGelir = all.Where(x => x.Status == "Onaylandı").Sum(x => x.TotalPrice);

            var values = string.IsNullOrEmpty(status)
                ? all
                : all.Where(x => x.Status == status).ToList();

            ViewBag.CurrentStatus = status;
            ViewBag.FilteredCount = values.Count;

            return View(values);
        }

        // ── Onayla + Mail Gönder ──
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var rental = await _rentalService.TGetByIdAsync(id);
            if (rental == null)
                return NotFound();

            rental.Status = "Onaylandı";
            await _rentalService.TUpdateAsync(rental);

            var customer = await _customerService.TGetByIdAsync(rental.CustomerId);

            if (customer != null && !string.IsNullOrEmpty(customer.Email))
            {
                var message = new MimeMessage();

                // Burayı kendi mail adresinizle değiştirin
                message.From.Add(new MailboxAddress("Rentaly", "isiktasyahya7@gmail.com"));
                message.To.Add(new MailboxAddress(
                    customer.Name + " " + customer.Surname,
                    customer.Email));

                message.Subject = "Rezervasyonunuz Onaylandı ✔";

                // -- 1. İndirim Kodunu (Sadece Metin Olarak) Oluştur --
                // Modelinizdeki alan isimlerini (TCKN, BirthDate) kontrol edin. Yoksa burayı atlar.
                string discountCode = "RENTALY10"; // Varsayılan kod
                try
                {
                    // TCKN ve BirthDate alanlarının varlığını kontrol et
                    var tcknProp = customer.GetType().GetProperty("TCKN");
                    var birthDateProp = customer.GetType().GetProperty("BirthDate");

                    if (tcknProp != null && birthDateProp != null)
                    {
                        var tckn = tcknProp.GetValue(customer)?.ToString();
                        var birthDateObj = birthDateProp.GetValue(customer);

                        if (!string.IsNullOrEmpty(tckn) && tckn.Length >= 2 && birthDateObj is DateTime birthDate)
                        {
                            string namePart = customer.Name.Substring(0, Math.Min(customer.Name.Length, 3)).ToUpper();
                            string tcknPart = tckn.Substring(0, 2);
                            string dobPart = birthDate.ToString("MMdd");

                            discountCode = $"{namePart}{tcknPart}{dobPart}";
                        }
                    }
                }
                catch
                {
                    // Hata durumunda varsayılan kod (RENTALY10) kullanılır
                }


                // -- 2. HTML Mail Gövdesini ve Görsel Eki Oluştur --
                // -- ── HTML Mail Gövdesini ve Görsel Eki Oluştur ── --
                var bodyBuilder = new BodyBuilder();

                // Sabit indirim görselini mailin içine gömelim
                // Dosyanın proje kök dizininde olduğunu varsayıyoruz.
                // Eğer wwwroot içindeyse yolu "wwwroot/images/discount_banner.png" yapın.
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "discount_banner.png");

                string imageContentId = "";
                if (System.IO.File.Exists(imagePath))
                {
                    var image = bodyBuilder.LinkedResources.Add(imagePath);
                    image.ContentId = MimeUtils.GenerateMessageId();
                    imageContentId = image.ContentId;
                }

                // CSS stilleri - Temanızla Uyumlu
                string styles = @"
    <style>
        /* Temel Stiller */
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color: #333; line-height: 1.6; background-color: #f6f8fa; margin: 0; padding: 0; }
        .container { width: 90%; max-width: 600px; margin: 20px auto; padding: 0; border: none; border-radius: 12px; background-color: #ffffff; box-shadow: 0 5px 20px rgba(0,0,0,0.05); overflow: hidden; }
        
        /* Header - Temanızın Üst Barına Benzer */
        .header { background-color: #1a1e21; color: #ffffff; padding: 25px 20px; text-align: left; display: flex; align-items: center; border-bottom: 4px solid #28a745; }
        .header-logo-container { flex: 0 0 50px; margin-right: 15px; }
        .header-logo { max-width: 50px; height: auto; }
        .header-title-container { flex: 1; }
        .header h1 { margin: 0; font-size: 1.8em; color: #ffffff; font-weight: bold; }
        .header h2 { margin: 3px 0 0; font-size: 0.9em; color: #aaaaaa; font-weight: normal; text-transform: uppercase; letter-spacing: 1px; }

        /* İçerik Alanı */
        .content { padding: 30px; }
        .content p { margin-bottom: 15px; font-size: 1.1em; color: #555; }
        .content .highlight-text { font-weight: bold; color: #28a745; }

        /* Detay Bloğu - İkonlu ve Temiz */
        .details-container { background-color: #f9f9f9; padding: 20px; border-radius: 8px; border: 1px solid #eee; margin: 20px 0; }
        .details-header { color: #333; margin-bottom: 15px; text-align: left; border-bottom: 1px solid #eee; padding-bottom: 10px; }
        .details-item { display: flex; align-items: center; margin-bottom: 10px; border-bottom: 1px solid #eee; padding-bottom: 10px; }
        .details-item:last-child { border-bottom: none; margin-bottom: 0; padding-bottom: 0;}
        .details-icon { flex: 0 0 30px; font-size: 1.5em; text-align: center; color: #28a745; margin-right: 15px; }
        .details-content { flex: 1; }
        .details-label { font-weight: bold; color: #555; display: block; font-size: 0.9em;}
        .details-value { font-weight: bold; color: #111; font-size: 1.2em; }
        .details-status-confirmed { color: #28a745; font-weight: bold; }

        /* İndirim Bölümü - Göz Alıcı ve Markalı */
        .discount-section { text-align: center; margin-top: 40px; padding: 25px; border-radius: 12px; background-color: #f1fff4; border: 2px solid #a3e6b1; box-shadow: 0 3px 10px rgba(40, 167, 69, 0.1); }
        .discount-section h3 { color: #1c7430; margin-top: 0; font-size: 1.6em; }
        .discount-image { max-width: 100%; height: auto; margin: 15px 0; border-radius: 8px; }
        .discount-code-block { margin: 20px auto; padding: 15px 25px; display: inline-block; border-radius: 6px; background-color: #ffffff; border: 2px dashed #28a745; }
        .discount-code-label { font-size: 0.9em; color: #777; margin-bottom: 5px; display: block; }
        .discount-code { font-family: 'Courier New', Courier, monospace; font-size: 2.8em; font-weight: bold; color: #28a745; letter-spacing: 3px; display: block; margin-top: 5px;}
        
        /* CTA Butonu - Sitenizdeki Yeşil Buton Gibi */
        .cta-button { display: inline-block; padding: 12px 30px; background-color: #28a745; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 4px; font-size: 1.1em; margin-top: 20px; text-transform: uppercase; border: 2px solid #28a745; transition: background-color 0.3s; }
        .cta-button:hover { background-color: #1e7e34; border-color: #1e7e34; }

        /* Footer */
        .footer { text-align: center; font-size: 0.85em; color: #999; margin-top: 40px; padding: 25px 30px; border-top: 1px solid #eee; background-color: #f9f9f9;}
        .footer p { margin: 0 0 10px; }
        .footer-logo { max-width: 30px; height: auto; margin-bottom: 10px; }
    </style>";

                string htmlBody = $@"
    <html>
    <head>
        {styles}
        <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css' />
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <div class='header-logo-container'>
                    <img src='https://via.placeholder.com/50x50/28a745/ffffff?text=R' alt='Logo' class='header-logo' />
                </div>
                <div class='header-title-container'>
                    <h1>Rentaly</h1>
                    <h2>Rezervasyonunuz Onaylandı!</h2>
                </div>
            </div>

            <div class='content'>
                <p>Merhaba <span class='highlight-text'>{customer.Name} {customer.Surname}</span>,</p>
                <p>Rezervasyon talebinizin başarıyla <span class='details-status-confirmed'>Onaylandığını</span> bildirmekten mutluluk duyarız. Araç kiralama detaylarınız aşağıdadır:</p>
                
                <div class='details-container'>
                    <h3 class='details-header'>Kiralama Özeti</h3>
                    
                    <div class='details-item'>
                        <div class='details-icon'><i class='fas fa-receipt'></i></div>
                        <div class='details-content'>
                            <span class='details-label'>Rezervasyon No:</span>
                            <span class='details-value'>#{rental.RentalId}</span>
                        </div>
                    </div>

                    <div class='details-item'>
                        <div class='details-icon'><i class='fas fa-calendar-alt'></i></div>
                        <div class='details-content'>
                            <span class='details-label'>Kiralama Tarihleri:</span>
                            <span class='details-value'>{rental.PickupDate:dd.MM.yyyy} - {rental.ReturnDate:dd.MM.yyyy}</span>
                        </div>
                    </div>

                    <div class='details-item'>
                        <div class='details-icon'><i class='fas fa-lira-sign'></i></div>
                        <div class='details-content'>
                            <span class='details-label'>Toplam Tutar:</span>
                            <span class='details-value'>{rental.TotalPrice:N0} ₺</span>
                        </div>
                    </div>

                    <div class='details-item'>
                        <div class='details-icon'><i class='fas fa-check-circle'></i></div>
                        <div class='details-content'>
                            <span class='details-label'>Durum:</span>
                            <span class='details-value details-status-confirmed'>Onaylandı</span>
                        </div>
                    </div>
                </div>

                <p>Keyifli bir sürüş dileriz.</p>
                
                <a href='#' class='cta-button'>Hemen Kiralamaya Başla</a>

                <div class='discount-section'>
                    <h3>Size Özel Hediye!</h3>
                    <p>Sonraki kiralamanızda geçerli %10 indiriminiz sizi bekliyor.</p>
                    
                    ";

                // Eğer görsel varsa ekle
                if (!string.IsNullOrEmpty(imageContentId))
                {
                    htmlBody += $"<img src='cid:{imageContentId}' alt='İndirim Kampanyası' class='discount-image' />";
                }

                // Dinamik kodu vurgulu bir şekilde CSS ile gösterelim
                htmlBody += $@"
                    <div class='discount-code-block'>
                        <span class='discount-code-label'>Kodunuz:</span>
                        <span class='discount-code'>{discountCode}</span>
                    </div>
                    <p style='font-size: 0.85em; color: #777; margin-top: 15px;'>Bu kod, sonraki kiralamanızda ödeme adımında kullanılabilir.</p>
                </div>

                <div class='footer'>
                    <img src='/RentalyTema/images/logo.png' alt='Logo' class='footer-logo' /><br/>
                    <p>Bizi tercih ettiğiniz için teşekkür ederiz.</p>
                    <p>&copy; 2023 Rentaly Araç Kiralama. Tüm hakları saklıdır.</p>
                    <p style='font-size: 0.8em;'>[Sitenizin Adresi] | [Telefon No]</p>
                </div>
            </div>
        </div>
    </body>
    </html>";

                bodyBuilder.HtmlBody = htmlBody;
                message.Body = bodyBuilder.ToMessageBody();

                // -- 3. Maili Gönder (Mevcut MailKit Kodunuz) --
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    // Uygulama şifresini buraya yazın
                    await client.AuthenticateAsync("isiktasyahya7@gmail.com", "ksvp nqsl dwpt ivfc");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }

            TempData["Success"] = "Rezervasyon onaylandı ve mail gönderildi!";
            return RedirectToAction("RentalList");
        }

        // ── Reddet (Burası Değişmedi) ──
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var rental = await _rentalService.TGetByIdAsync(id);
            if (rental == null)
                return NotFound();

            rental.Status = "İptal";
            await _rentalService.TUpdateAsync(rental);

            TempData["Error"] = "Rezervasyon reddedildi!";
            return RedirectToAction("RentalList");
        }
    }
}