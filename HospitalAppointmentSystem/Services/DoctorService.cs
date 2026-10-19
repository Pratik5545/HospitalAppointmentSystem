using HospitalAppointmentSystem.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HospitalAppointmentSystem.Services;

public class DoctorService
{
    private readonly IMemoryCache _cache;
    private const string CacheKey = "doctorList";

    public DoctorService(IMemoryCache cache) => _cache = cache;

    public List<Doctor> GetDoctors()
    {
        return _cache.GetOrCreate(CacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3);
            Console.WriteLine("[CACHE MISS] Doctor list loaded from source (cached for 3 minutes)");
            return new List<Doctor>
            {
                new() { DoctorId = 1, DoctorName = "Dr. Anil Mehta",   Specialization = "Cardiology",  Experience = 15, ConsultationFee = 800 },
                new() { DoctorId = 2, DoctorName = "Dr. Priya Shah",   Specialization = "Dermatology", Experience = 8,  ConsultationFee = 500 },
                new() { DoctorId = 3, DoctorName = "Dr. Rahul Patel",  Specialization = "Orthopedics", Experience = 12, ConsultationFee = 700 },
                new() { DoctorId = 4, DoctorName = "Dr. Neha Desai",   Specialization = "Pediatrics",  Experience = 6,  ConsultationFee = 400 },
                new() { DoctorId = 5, DoctorName = "Dr. Kiran Joshi",  Specialization = "Cardiology",  Experience = 20, ConsultationFee = 1000 }
            };
        })!;
    }

    public Doctor? GetById(int id) => GetDoctors().FirstOrDefault(d => d.DoctorId == id);
}
