using System.IO.Pipes;
using System.Runtime.CompilerServices;

public struct Structure
{
    public int n;
    public int m;
}

class Server
{
    static void Main()
    {
        // Открытие канала
        using NamedPipeServerStream Server = new("channel", PipeDirection.InOut);
        Server.WaitForConnection();

        // Создание сообщения
        Structure msg = new()
        {
            n = 99,
            m = 10,
        };
        
        // Преобразование в байты
        byte[] bytes = new byte[Unsafe.SizeOf<Structure>()];
        Unsafe.As<byte, Structure>(ref bytes[0]) = msg;
        Server.Write(bytes, 0, bytes.Length);

        // Получение измененных данных от клиента
        byte[] receivedBytes = new byte[Unsafe.SizeOf<Structure>()];
        Server.Read(receivedBytes, 0, receivedBytes.Length);
        Structure receivedData = Unsafe.As<byte, Structure>(ref receivedBytes[0]);
        Console.WriteLine($"Received data: n = {receivedData.n}, m = {receivedData.m}");
    }
}