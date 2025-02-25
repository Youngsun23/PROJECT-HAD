using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HAD
{
    public class CharacterCommandManager : MonoBehaviour
    {
        public Queue<ICommand> CharCommandQueue;
        public ICommand singleCommand;
        readonly CommandInvoker commandInvoker;
        public PlayerCharacter characterBase;
        private Animator animator;

        public CharacterCommandManager()
        {
            CharCommandQueue = new Queue<ICommand>();
            commandInvoker = new CommandInvoker();
        }

        private void Start()
        {
            characterBase = GetComponent<PlayerCharacter>();
            animator = GetComponent<Animator>();
        }

        public void AddCommand(int num)
        {
            var command = CommandFactory.CreateCommand(characterBase, num);

            if (command != null)
            {
                if (CharCommandQueue.Count >= 1)
                {
                    return;
                }

                CharCommandQueue.Enqueue(command);

                if (num == 0)
                {
                    CharCommandQueue.Dequeue();
                    command.Execute();
                }
            }
        }

        public void ExecuteCommand()
        {
            if (CharCommandQueue.Count > 0)
            {
                var command = CharCommandQueue.Dequeue();
                command.Execute();
            }
        }

        private void Update()
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 0.9f)
            {
                ExecuteCommand();
            }
        }

        public class CommandInvoker
        {
            public async Task ExecuteCommand(Queue<ICommand> commands)
            {
                foreach (var command in commands)
                {
                    await command.Execute();
                }
            }
        }
    }
}
