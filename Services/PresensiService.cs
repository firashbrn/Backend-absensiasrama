// Services/PresensiService.cs
using App.Models;

namespace App.Services
{
    public class PresensiService
    {
        private const double AsramaLat = -0.914500;
        private const double AsramaLng = 100.465400;
        private const double RadiusMeter = 100;

        public (bool valid, string pesan, SesiPresensi? sesi) ValidasiWaktu()
        {
            var jamSekarang = DateTime.Now.TimeOfDay;

            if (jamSekarang >= new TimeSpan(4, 0, 0) && jamSekarang <= new TimeSpan(6, 0, 0))
                return (true, "OK", SesiPresensi.Subuh);

            if (jamSekarang >= new TimeSpan(18, 0, 0) && jamSekarang <= new TimeSpan(20, 30, 0))
                return (true, "OK", SesiPresensi.Malam);

            return (false, "Di luar jam presensi (Subuh 04.00-06.00 / Malam 18.00-20.30)", null);
        }

        public (bool valid, string pesan) ValidasiLokasi(double lat, double lng)
        {
            var jarak = HitungJarakMeter(AsramaLat, AsramaLng, lat, lng);
            if (jarak > RadiusMeter)
                return (false, $"Di luar area asrama (jarak {jarak:F0}m, maks {RadiusMeter}m)");
            return (true, "OK");
        }

        private double HitungJarakMeter(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}