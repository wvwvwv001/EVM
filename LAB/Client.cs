using System.IO.Pipes;
using System.Runtime.CompilerServices;

public struct Structure
{
    public int n;
    public int m;
}

class Client
{
    static void Main()
    {
        // Открытие канала
        using NamedPipeClientStream Client = new(".", "channel", PipeDirection.InOut);
        Client.Connect();

        // Получение данных от сервера
        byte[] bytes = new byte[Unsafe.SizeOf<Structure>()];
        Client.Read(bytes, 0, bytes.Length);
        Structure receivedData = Unsafe.As<byte, Structure>(ref bytes[0]);
        Console.WriteLine($"Received data: n = {receivedData.n}, m = {receivedData.m}");

        // Изменение флага
        receivedData.n *= receivedData.m;
        
        // Отправка измененных данных обратно на сервер
        byte[] modified_bytes = new byte[Unsafe.SizeOf<Structure>()];
        Unsafe.As<byte, Structure>(ref modified_bytes[0]) = receivedData;
        Client.Write(modified_bytes, 0, modified_bytes.Length);
    }
}