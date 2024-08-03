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
        // public List<ICommand> charCommandList;
        // Class Try 2 _ Command Pattern
        public ICommand singleCommand;
        readonly CommandInvoker commandInvoker;
        public CharacterBase characterBase;
        //bool isNextCommandReady;
        private Animator animator;

        //public bool IsNextCommandReady => isNextCommandReady;

        public CharacterCommandManager()
        {
            CharCommandQueue = new Queue<ICommand>();
            commandInvoker = new CommandInvoker();
        }

        private void Start()
        {
            characterBase = GetComponent<CharacterBase>();
            //singleCommand = CommandFactory.CreateCommand(characterBase, 0);
            animator = GetComponent<Animator>();
        }

        // 영상
        //private void Start()
        //{
        //    characterBase = GetComponent<CharacterBase>();

        //    singleCommand = AttackComboCommands.Create<AttackCombo1Command>(characterBase);

        //    charCommandList = new List<ICommand>
        //    {
        //        AttackComboCommands.Create<AttackCombo1Command>(characterBase),
        //        AttackComboCommands.Create<AttackCombo2Command>(characterBase),
        //        AttackComboCommands.Create<AttackCombo3Command>(characterBase)
        //    };
        //}

        //public void AddCommand(ICommand newCommand)
        //{
        //    CharCommandQueue.Enqueue(newCommand);
        //    newCommand.Execute();
        //}
        public void AddCommand(int num)
        {
            var command = CommandFactory.CreateCommand(characterBase, num);
            //Debug.Log("command: "+command);

            if (command != null)
            {
                // 이렇게 하면 1 중에 들어온 입력만 2로 연결, 2 중에 들어온 입력만 3으로 연결해서 문제는 해결인데
                // 이럴거면 Queue를 쓰는 의미가 없잖냐
                if (CharCommandQueue.Count >= 1 /*&& command == CharCommandQueue.Peek()*/)
                {
                    // Debug.Log($"똑같냐? {command == CharCommandQueue.Peek()}"); // false
                    return;
                }

                CharCommandQueue.Enqueue(command);
                // Debug.Log($"커맨드 Enqueue: {command}");
                // Debug.Log("Peek command: " + CharCommandQueue.Peek());

                // 이 부분이 문제인가? 
                if (num == 0)
                {
                    CharCommandQueue.Dequeue();
                    command.Execute();
                }

                //if(CharCommandQueue.Count > 0)
                //{
                //    characterBase.SetNextComboReady(true);
                //}

                // commandInvoker.ExecuteCommand(CharCommandQueue);
            }
        }

        // 지연시키기 -> Execute를 큐 관리 함수 쪽에 따로 빼고, CharacterBase업데이트로 돌려야 하나?
        public void ExecuteCommand()
        {
            if(CharCommandQueue.Count > 0)
            {
                var command = CharCommandQueue.Dequeue();
                //if(CharCommandQueue.Count <= 0)
                //{
                //    characterBase.SetNextComboReady(false);
                //}
                command.Execute();
                //Debug.Log($"커맨드 Dequeue&Execute: {command}");
            }
        }

        private void Update()
        {
            // 지연시키기
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 0.9f)
            {
                ExecuteCommand();
            }
        }

        // 영상
        //public async Task ExecuteCommand(List<ICommand> commands)
        //{
        //    isCommandExecuting = true;
        //    await commandInvoker.ExecuteCommand(commands);
        //    isCommandExecuting = false;
        //}

        // 여기 Update에서 인풋(키1||키2)에 따라
        // singleCommand 하나 담은 리스트 새로 만들어 ExecuteCommand()하거나
        // 보유한 액션 모두 담아뒀던 commands를 ExeucteCommand()하는 방식
        // 입력-공1-입력-공2-입력-공3의 형태와 맞지 않음
    }

    // 영상
    public class CommandInvoker
    {
        public async Task ExecuteCommand(Queue<ICommand> commands)
        {
            foreach(var command in commands)
            {
                //commands.Dequeue();
                await command.Execute();
            }
        }
    }
}
