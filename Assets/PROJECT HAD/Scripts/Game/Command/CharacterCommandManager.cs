using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// @ Invoker : 손님 주문 받아서 해당 레시피 쟁여놓고 요리사에게 전달
namespace HAD
{
    public class CharacterCommandManager : MonoBehaviour
    {
        public Queue<ICommand> CharCommandQueue;
        // Class Try 2 _ Command Pattern
        public ICommand singleCommand;
        readonly CommandInvoker commandInvoker;

        public CharacterCommandManager()
        {
            CharCommandQueue = new Queue<ICommand>();
        }

        public void AddCommand(ICommand newCommand)
        {
            CharCommandQueue.Enqueue(newCommand);
            newCommand.Execute();
        }

        // Class Try 2 _ Command Pattern
        async Task ExecuteCommand(List<ICommand> commands)
        {
            await commandInvoker.ExecuteCommand(commands);
        }
    }

    // Class Try 2 _ Command Pattern
    public class CommandInvoker
    {
        public async Task ExecuteCommand(List<ICommand> commands)
        {
            foreach(var command in commands)
            {
                await command.Execute();
            }
        }
    }
}
