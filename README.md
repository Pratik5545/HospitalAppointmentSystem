# Hospital Appointment Management System (TASK A)

An ASP.NET Core MVC application developed with **.NET 10** to manage doctor information, schedule patient appointments, fetch asynchronous bulletins, and demonstrate advanced state management and caching techniques.

---

## 📋 Features Implemented

### 1. Model & Extension Method
- **`Doctor` Model** (`Models/Doctor.cs`): Defines `DoctorId`, `DoctorName`, `Specialization`, `Experience`, and `ConsultationFee`.
- **`CalculateTotalCharge()` Extension Method** (`Extensions/DoctorExtensions.cs`): Adds a ₹100 mandatory service charge (`ConsultationFee + 100`).
- **Display**: Displays consultation fee, service fee, and total charges across directory and details pages.

### 2. Asynchronous Web Request
- **`Post` Model** (`Models/Post.cs`) & **`PostsController`** (`Controllers/PostsController.cs`):
  - Fetches 100 public bulletin posts asynchronously from `https://jsonplaceholder.typicode.com/posts` using `IHttpClientFactory` and `client.GetFromJsonAsync<List<Post>>()`.
  - Displays Post ID and Title in a responsive, styled table.

### 3. State Management
- **Cookie**: Remembers and persists the patient's name across visits (stored for 7 days via `Response.Cookies.Append("PatientName", ...)`).
- **Session**: Stores the user's selected specialization filter (`HttpContext.Session.SetString("Specialization", ...)`).
- **Query String**: Transmits `doctorId` to the Doctor Details page (`/Doctors/Details?doctorId=1`).
- **Hidden Field**: Submits `doctorId` securely inside the appointment booking form (`<input type="hidden" name="doctorId" value="@Model.DoctorId" />`).

### 4. Caching
- **Response Caching**: Applied to the Doctor Details action using `[ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "doctorId" })]` with `app.UseResponseCaching()`.
- **In-Memory Caching (`IMemoryCache`)**: Caches the doctor list in memory for 3 minutes using `entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)`.

---

## 🚀 How to Run Locally

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (`dotnet --version` should show `10.x`)
- [Visual Studio Code](https://code.visualstudio.com/) with the **C# Dev Kit** extension.

### Running from Terminal
```bash
# Navigate to the project folder
cd HospitalAppointmentSystem

# Run the application
dotnet run
```
Open your browser and navigate to:
```
http://localhost:5213
```
*(or the port printed in your terminal)*

---

## 📂 Project Architecture

```
Web_NET/
├── HospitalAppointmentSystem.sln     # Solution file
├── README.md                          # Documentation
└── HospitalAppointmentSystem/
    ├── Controllers/
    │   ├── DoctorsController.cs       # State management, Caching, Booking
    │   ├── PostsController.cs         # Asynchronous HTTP requests
    │   └── HomeController.cs
    ├── Extensions/
    │   └── DoctorExtensions.cs        # CalculateTotalCharge() (+₹100 charge)
    ├── Models/
    │   ├── Doctor.cs                  # Doctor model
    │   ├── Post.cs                    # Bulletin post model
    │   └── ErrorViewModel.cs
    ├── Services/
    │   └── DoctorService.cs           # IMemoryCache (3-min expiration)
    ├── Views/
    │   ├── Doctors/
    │   │   ├── Index.cshtml           # Directory & Preference controls
    │   │   ├── Details.cshtml         # Details (Query string & Hidden field)
    │   │   └── Confirmation.cshtml    # Booking confirmation
    │   ├── Posts/
    │   │   └── Index.cshtml           # External posts table
    │   └── Shared/
    │       └── _Layout.cshtml         # Navigation & modern theme styling
    └── Program.cs                     # Middleware & DI registration
```
