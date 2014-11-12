namespace RED.Models
{
    using Interfaces;

    public class ModuleGridModel
    {
        internal string _leftSelection;
        internal string _rightSelection;
        internal string _topSelection;
        internal string _middleSelection;
        internal string _bottomSelection;
        internal IModule _leftModule;
        internal IModule _rightModule;
        internal IModule _topModule;
        internal IModule _middleModule;
        internal IModule _bottomModule;
        internal string _column1Width = "1*";
        internal string _column3Width = "2*";
        internal string _column5Width = "1*";
        internal string _row1Height = "1*";
        internal string _row3Height = "1*";
        internal string _row5Height = "1*";
    }
}