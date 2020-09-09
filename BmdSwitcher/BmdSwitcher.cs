using BMDSwitcherAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BmdSwitcher
{
    public class BmdSwitcher : Switcher
    {
        private IBMDSwitcher _switcher;
        private IBMDSwitcherMixEffectBlock _effectBlock;

        public void Connect(string deviceAddress)
        {
            var discovery = new BMDSwitcherAPI.CBMDSwitcherDiscoveryClass();
            discovery.ConnectTo(deviceAddress, out this._switcher, out var reason);

            this.Inputs = this.Iterate<IBMDSwitcherInputIterator, IBMDSwitcherInput>(iterator => iterator.Next).Select(GetInput).ToList();

            // ATEM Mini Pro only has one MixEffectBlock
            this._effectBlock = Iterate<IBMDSwitcherMixEffectBlockIterator, IBMDSwitcherMixEffectBlock>(iterator => iterator.Next).First();

            this.updateCurrentProgramInput();
            _effectBlock.AddCallback(new EffectBlockHandler(this));
        }

        SwitcherInput GetInput(IBMDSwitcherInput comInput)
        {
            comInput.GetLongName(out var name);
            comInput.GetInputId(out var id);

            return new SwitcherInput
            {
                Id = id,
                Name = name
            };
        }

        public string ProductName
        {
            get
            {
                _switcher.GetProductName(out string name); return name;
            }
        }

        public IList<SwitcherInput> Inputs { get; private set; }
        public Guid Id { get; } = Guid.NewGuid();

        protected void updateCurrentProgramInput()
        {
            this._effectBlock.GetProgramInput(out var value);
            this.CurrentProgramInput = this.Inputs.First(input => input.Id == value);
        }

        public SwitcherInput CurrentProgramInput { get; protected set; }

        delegate void Iterator<T>(out T next);

        IEnumerable<TElement> Iterate<TIterator, TElement>(Func<TIterator, Iterator<TElement>> nextSelector)
        {
            var guid = typeof(TIterator).GUID;

            _switcher.CreateIterator(ref guid, out var ptr);
            TIterator iterator = (TIterator)Marshal.GetObjectForIUnknown(ptr);

            TElement getNext()
            {
                nextSelector(iterator)(out TElement result);
                return result;
            }

            while (true)
            {
                TElement item = getNext();

                if (item == null) break;

                yield return item;
            }
        }


        class EffectBlockHandler : IBMDSwitcherMixEffectBlockCallback
        {
            private readonly BmdSwitcher switcher;

            public EffectBlockHandler(BmdSwitcher switcher)
            {
                this.switcher = switcher;
            }
            public void Notify(_BMDSwitcherMixEffectBlockEventType eventType)
            {
                switch(eventType)
                {
                    case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeProgramInputChanged:
                        this.switcher.updateCurrentProgramInput();
                        break;
                }
            }
        }

    }


}
