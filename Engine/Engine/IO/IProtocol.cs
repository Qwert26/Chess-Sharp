using System;
namespace Engine.IO
{
    public interface IProtocol
    {
        Action ParseCommand(string input);
    }
}
