using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @ Invoker : 손님 주문 받아서 해당 레시피 쟁여놓고 요리사에게 전달
namespace HAD
{
    public class CharacterCommandInvoker
    {
        Queue<ICommand> CharCommandQueue;

        public CharacterCommandInvoker()
        {
            CharCommandQueue = new Queue<ICommand>();
        }

        public void AddCommand(ICommand newCommand)
        {
            CharCommandQueue.Enqueue(newCommand);
            newCommand.Execute();
        }
    }
}
