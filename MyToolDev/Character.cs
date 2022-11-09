using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToolDev
{
    [Serializable]
   public  class Character : INotifyPropertyChanged, ICloneable
    {
        private string _name = " ";
        public string Name { get => _name; set => OnPropertyChanged(ref _name, value, nameof(Name)); }

        private bool _isMale;
        public bool IsMale { get => _isMale; set => OnPropertyChanged(ref _isMale, value, nameof(IsMale)); }

        private int _health;
        public int Health { get => _health; set => OnPropertyChanged(ref _health, value, nameof(Health)); }

        private int _defence;
        public int Defence { get => _defence; set => OnPropertyChanged(ref _defence, value, nameof(Defence)); }

        private int _attack;
        public int Attack { get => _attack; set => OnPropertyChanged(ref _attack, value, nameof(Attack)); }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;


        public Character()
        {


        }

        public Character(Character other)
        {
            _name = other._name;
            _isMale = other._isMale;
            _health = other._health;
            _attack = other._attack;
            _defence = other._defence;
        }






        private void OnPropertyChanged<T>(ref T oldValue, T newValue, string propertyName)
        {


            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{Name} - {IsMale} - {Health} - {Attack} - {Defence}";
        }

        public object Clone()
        {
            return new Character(this);
        }
    }
}
