using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.Concrete;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.EntityFreamework;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// SERVICE REGISTER
// ======================================================

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<ICarDal, EfCarDal>();
builder.Services.AddScoped<ICarService, CarManager>();

builder.Services.AddScoped<ICarModelDal, EfCarModelDal>();
builder.Services.AddScoped<ICarModelService, CarModelManager>();

builder.Services.AddScoped<IBrandDal, EfBrandDal>();
builder.Services.AddScoped<IBrandService, BrandManager>();

builder.Services.AddScoped<IBranchDal, EfBranchDal>();
builder.Services.AddScoped<IBranchService, BranchManager>();

builder.Services.AddScoped<ICustomerDal, EfCustomerDal>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();

builder.Services.AddScoped<IRentalDal, EfRentalDal>();
builder.Services.AddScoped<IRentalService, RentalManager>();

builder.Services.AddScoped<IProcessDal, EfProcessDal>();
builder.Services.AddScoped<IProcessService, ProcessManager>();

builder.Services.AddScoped<IFeatureDal, EfFeatureDal>();
builder.Services.AddScoped<IFeatureService, FeatureManager>();

builder.Services.AddScoped<IAboutDal, EfAboutDal>();
builder.Services.AddScoped<IAboutService, AboutManager>();

builder.Services.AddScoped<IWhyChooseUsDal, EfWhyChooseUsDal>();
builder.Services.AddScoped<IWhyChooseUsService, WhyChooseUsManager>();

builder.Services.AddScoped<ITestiominalDal, EfTestiominalDal>();
builder.Services.AddScoped<ITestiominalService, TestiominalManager>();

builder.Services.AddScoped<IFaqDal, EfFaqDal>();
builder.Services.AddScoped<IFaqService, FaqManager>();

// ======================================================
// OTHER
// ======================================================

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<RentalyContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ======================================================
// ERROR HANDLING
// ======================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}

// ======================================================
// MIDDLEWARE
// ======================================================

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 404 SAYFASI
app.UseStatusCodePagesWithReExecute("/Error/404");

// ======================================================
// ROUTES
// ======================================================

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();