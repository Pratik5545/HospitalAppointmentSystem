using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.Controllers;

public class DoctorsController : Controller
{
    private readonly DoctorService _service;

    public DoctorsController(DoctorService service)
    {
        _service = service;
    }

    // Displays the list of doctors, patient name from Cookie, and filters by Specialization from Session
    public IActionResult Index()
    {
        var allDoctors = _service.GetDoctors();

        // State Management - Session: Retrieve stored specialization
        var selectedSpec = HttpContext.Session.GetString("Specialization");

        // Filter list if specialization is stored in Session
        var filteredDoctors = string.IsNullOrEmpty(selectedSpec)
            ? allDoctors
            : allDoctors.Where(d => d.Specialization.Equals(selectedSpec, StringComparison.OrdinalIgnoreCase)).ToList();

        // State Management - Cookie: Retrieve patient name
        ViewBag.PatientName = Request.Cookies["PatientName"];
        ViewBag.Specialization = selectedSpec;
        ViewBag.Specializations = allDoctors.Select(d => d.Specialization).Distinct().OrderBy(s => s).ToList();

        return View(filteredDoctors);
    }

    // State Management - Saves Patient Name in Cookie and Specialization in Session
    [HttpPost]
    public IActionResult SetPreferences(string? patientName, string? specialization)
    {
        // 1. Cookie: Store patient name (7 days expiration)
        if (!string.IsNullOrWhiteSpace(patientName))
        {
            Response.Cookies.Append("PatientName", patientName.Trim(), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                IsEssential = true
            });
        }
        else
        {
            Response.Cookies.Delete("PatientName");
        }

        // 2. Session: Store selected specialization
        if (!string.IsNullOrWhiteSpace(specialization))
        {
            HttpContext.Session.SetString("Specialization", specialization.Trim());
        }
        else
        {
            HttpContext.Session.Remove("Specialization");
        }

        return RedirectToAction(nameof(Index));
    }

    // Action to clear patient preferences
    [HttpPost]
    public IActionResult ClearPreferences()
    {
        Response.Cookies.Delete("PatientName");
        HttpContext.Session.Remove("Specialization");
        return RedirectToAction(nameof(Index));
    }

    // State Management: Query String (/Doctors/Details?doctorId=1)
    // Caching: Response Caching for 60 seconds varied by query parameter 'doctorId'
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "doctorId" }, Location = ResponseCacheLocation.Any)]
    public IActionResult Details(int doctorId)
    {
        var doctor = _service.GetById(doctorId);
        if (doctor == null)
        {
            return NotFound($"Doctor with ID {doctorId} not found.");
        }

        ViewBag.PatientName = Request.Cookies["PatientName"] ?? "Guest";
        return View(doctor);
    }

    // State Management: Hidden Field submits doctorId with the appointment request
    [HttpPost]
    public IActionResult Book(int doctorId, DateTime appointmentDate, string reason)
    {
        var doctor = _service.GetById(doctorId);
        if (doctor == null)
        {
            return NotFound();
        }

        ViewBag.PatientName = Request.Cookies["PatientName"] ?? "Guest";
        ViewBag.AppointmentDate = appointmentDate;
        ViewBag.Reason = reason;

        return View("Confirmation", doctor);
    }
}
