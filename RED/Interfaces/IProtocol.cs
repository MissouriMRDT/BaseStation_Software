using System.Threading.Tasks;

namespace RED.Interfaces
{
    public interface IProtocol
    {
        Task Connect(IConnection source);
    }
}