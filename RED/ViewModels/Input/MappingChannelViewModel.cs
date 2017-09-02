using Caliburn.Micro;
using RED.Contexts.Input;
using RED.Models.Input;

namespace RED.ViewModels.Input
{
    public class MappingChannelViewModel : PropertyChangedBase
    {
        private readonly MappingChannelModel _model;

        public string InputKey
        {
            get
            {
                return _model.InputKey;
            }
            set
            {
                _model.InputKey = value; NotifyOfPropertyChange(() => InputKey);
            }

        }
        public string OutputKey
        {
            get
            {
                return _model.OutputKey;
            }
            set
            {
                _model.OutputKey = value; NotifyOfPropertyChange(() => OutputKey);
            }

        }
        public float LinearScaling
        {
            get
            {
                return _model.LinearScaling;
            }
            set
            {
                _model.LinearScaling = value; NotifyOfPropertyChange(() => LinearScaling);
            }

        }
        public bool Parabolic
        {
            get
            {
                return _model.Parabolic;
            }
            set
            {
                _model.Parabolic = value; NotifyOfPropertyChange(() => Parabolic);
            }

        }
        public float Minimum
        {
            get
            {
                return _model.Minimum;
            }
            set
            {
                _model.Minimum = value; NotifyOfPropertyChange(() => Minimum);
            }

        }
        public float Maximum
        {
            get
            {
                return _model.Maximum;
            }
            set
            {
                _model.Maximum = value; NotifyOfPropertyChange(() => Maximum);
            }

        }
        public float Offset
        {
            get
            {
                return _model.Offset;
            }
            set
            {
                _model.Offset = value; NotifyOfPropertyChange(() => Offset);
            }

        }

        private MappingChannelViewModel()
        {
            _model = new MappingChannelModel();
        }

        public MappingChannelViewModel(string inputKey, string outputKey)
            : this()
        {
            InputKey = inputKey;
            OutputKey = outputKey;
        }

        public MappingChannelViewModel(InputChannelContext context)
            : this()
        {
            InputKey = context.InputKey;
            OutputKey = context.OutputKey;
            LinearScaling = context.LinearScaling;
            Parabolic = context.Parabolic;
            Minimum = context.Minimum;
            Maximum = context.Maximum;
            Offset = context.Offset;
        }

        public float Map(float input)
        {
            if (Parabolic)
                input = input * input * (input >= 0 ? 1 : -1);

            float result = input * LinearScaling + Offset;

            if (result < Minimum) result = Minimum;
            if (result > Maximum) result = Maximum;

            return result;
        }

        public InputChannelContext ToContext()
        {
            return new InputChannelContext()
            {
                InputKey = InputKey,
                OutputKey = OutputKey,
                LinearScaling = LinearScaling,
                Parabolic = Parabolic,
                Minimum = Minimum,
                Maximum = Maximum,
                Offset = Offset
            };
        }
    }
}
