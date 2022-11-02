namespace MARO.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key) : base($"Объект \"{name}\" с ключом ({key}) не найден") { }
    }
}
