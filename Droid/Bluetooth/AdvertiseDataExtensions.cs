using Android.OS;
using Android.Bluetooth.LE;

using Java.Util;

namespace XamarinUtils.Droid
{
    public static class AdvertiseDataExtensions
    {
        const int Uuid16BitBytes = 2;
        const int Uuid32BitBytes = 4;
        const int Uuid128BitBytes = 16;
        const int FlagsFieldBytes = 3;
        const int FieldOverheadBytes = 2;
        const int ServiceDataUuidLength = 2;
        const int ManufacturerDataLength = 2;

        public const int MaxAdvertisingDataBytes = 31;

        public static ParcelUuid BASE_UUID = ParcelUuid.FromString ("00000000-0000-1000-8000-00805F9B34FB");

        static int TotalBytes (this AdvertiseData data, string adapterName)
        {
            int size = 0;

            if (data == null) return size;

            if (data.ServiceUuids != null) {
                int num16BitUuids = 0;
                int num32BitUuids = 0;
                int num128BitUuids = 0;

                foreach (ParcelUuid uuid in data.ServiceUuids) {
                    if (Is16BitUuid (uuid)) {
                        ++num16BitUuids;
                    } else if (Is32BitUuid (uuid)) {
                        ++num32BitUuids;
                    } else {
                        ++num128BitUuids;
                    }
                }

                // 16 bit service uuids are grouped into one field when doing advertising.
                if (num16BitUuids != 0) {
                    size += FieldOverheadBytes + num16BitUuids * Uuid16BitBytes;
                }

                // 32 bit service uuids are grouped into one field when doing advertising.
                if (num32BitUuids != 0) {
                    size += FieldOverheadBytes + num32BitUuids * Uuid32BitBytes;
                }

                // 128 bit service uuids are grouped into one field when doing advertising.
                if (num128BitUuids != 0) {
                    size += FieldOverheadBytes + num128BitUuids * Uuid128BitBytes;
                }
            }

            foreach (ParcelUuid uuid in data.ServiceData.Keys) {
                size += FieldOverheadBytes + ServiceDataUuidLength + ByteLength (data.ServiceData[uuid]);
            }

            for (int i = 0; i < data.ManufacturerSpecificData.Size (); ++i) {
                size += FieldOverheadBytes +
                        ManufacturerDataLength +
                        ByteLength ((byte[])data.ManufacturerSpecificData.ValueAt (i));
            }

            if (data.IncludeTxPowerLevel) {
                // tx power level value is one byte.
                size += FieldOverheadBytes + 1;
            }

            if (data.IncludeDeviceName && adapterName != null) {
                size += FieldOverheadBytes + adapterName.Length;
            }

            // Flags field is omitted if the advertising is not connectable.
            if (size + FlagsFieldBytes <= MaxAdvertisingDataBytes) {
                size += FlagsFieldBytes;
            }

            return size;
        }

        static bool Is16BitUuid (ParcelUuid parcelUuid)
        {
            return Is16BitUuid (parcelUuid.Uuid);
        }

        static bool Is16BitUuid (UUID uuid)
        {
            if (uuid.LeastSignificantBits != BASE_UUID.Uuid.LeastSignificantBits) {
                return false;
            }

            return (((ulong)uuid.MostSignificantBits & 0xFFFF0000FFFFFFFFL) == 0x1000L);
        }

        static bool Is32BitUuid (ParcelUuid parcelUuid)
        {
            return Is32BitUuid (parcelUuid.Uuid);
        }

        static bool Is32BitUuid (UUID uuid)
        {
            if (uuid.LeastSignificantBits != BASE_UUID.Uuid.LeastSignificantBits) {
                return false;
            }

            if (Is16BitUuid (uuid)) {
                return false;
            }

            return ((uuid.MostSignificantBits & 0xFFFFFFFFL) == 0x1000L);
        }

        static int ByteLength (byte[] array)
        {
            return array == null ? 0 : array.Length;
        }
    }
}