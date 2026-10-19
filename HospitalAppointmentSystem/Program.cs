using HospitalAppointmentSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 4. Caching: IMemoryCache registration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();

// Register application services
builder.Services.AddScoped<DoctorService>();

// 2. Asynchronous Web Request: HttpClient registration
builder.Services.AddHttpClient();

// 3. State Management: Session service configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 4. Caching: Response Caching service registration
builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Middleware: Session and Response Caching must follow UseRouting
app.UseSession();
app.UseResponseCaching();

app.UseAuthorization();

app.MapStaticAssets();

// Set default controller to Doctors
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Doctors}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
