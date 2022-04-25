using System;

namespace Student_Subject_Evaluation.MVVM.View_Model
{
    internal class AnotherCommandImplementation
    {
        private Action<object> p;

        public AnotherCommandImplementation(Action<object> p)
        {
            this.p = p;
        }
    }
}