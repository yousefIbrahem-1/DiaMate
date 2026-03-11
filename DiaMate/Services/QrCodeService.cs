using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;


public class QrCodeService : IQrCodeService
{
    public byte[] GenerateQrCode(string text)
    {
        QRCodeGenerator generator = new QRCodeGenerator();
        QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

        PngByteQRCode qrCode = new PngByteQRCode(data);

        return qrCode.GetGraphic(20);
    }
}