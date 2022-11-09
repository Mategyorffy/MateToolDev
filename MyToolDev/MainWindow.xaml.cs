using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Microsoft.Win32;
using MyToolDev;

namespace MyToolDev
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DIALOG_FILTER = "Data Files|*.dat;*.bin;*.char|All Files|*-*";
        private ObservableCollection<Character> _characters = new ObservableCollection<Character>();
        private Character _defaultCharacter;
        bool _isCharacterChanged;
        string _lastCharacterPath;


        public MainWindow()
        {
            InitializeComponent();

            _characters.Add(new Character { Name = "Slippin Jim", IsMale = true, Attack = 6, Health = 16, Defence = 100 });

            CharactersListBox.ItemsSource = _characters;
            _defaultCharacter = FindResource("DefaultCharacter") as Character;
            
        }

        public static RoutedUICommand CostumOpenCommand = new RoutedUICommand("_Open", "CostumOpen", typeof(MainWindow), new InputGestureCollection
        {


            new KeyGesture(Key.A, ModifierKeys.Alt)

        });


        public static RoutedUICommand AddCharacterCommand = new RoutedUICommand("Add Character", "AddCharacter", typeof(MainWindow));

        public static RoutedUICommand RemoveCharacterCommand = new RoutedUICommand("Remove Character", "RemoveCharacter", typeof(MainWindow));

        public static RoutedUICommand ChangeCharacterCommand = new RoutedUICommand("Change Character", "ChangeCharacter", typeof(MainWindow));

        public static RoutedUICommand RevertCharacterCommand = new RoutedUICommand("Revert Character", "RevertCharacter", typeof(MainWindow));


        private void AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void OnShowSenderCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }

        private void OnAddCharacterCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ItemWindow itemWindow = new ItemWindow();
            itemWindow.Owner = this;
            itemWindow.Title = "ADD new Character";
            Character character = new Character();
            itemWindow.DataContext = character;
            if (itemWindow.ShowDialog()?? false)
            {
                _characters.Add(character);
            }
        }

        private void CanCharacterListChangeExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CharactersListBox.SelectedIndex >= 0 && CharactersListBox.SelectedIndex < _characters.Count;
        }

        private void OnRemoveCharacterCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _characters.RemoveAt(CharactersListBox.SelectedIndex);
        }

        private void OnChangeCharacterCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ItemWindow itemWindow = new ItemWindow();
            itemWindow.Owner = this;
            itemWindow.Title = "Change Character";
            Character character = new Character(_characters[CharactersListBox.SelectedIndex]);
            itemWindow.DataContext = character;
            if(itemWindow.ShowDialog()?? false)
            {
                _characters[CharactersListBox.SelectedIndex] = character;
            }
        }
     

        private void CanCharacterChangeExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _isCharacterChanged;
        }

        private void OnCharacterChangeExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }


        private void OnOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = DIALOG_FILTER;

            if(ofd.ShowDialog()?? false)
            {
                string filename = ofd.FileName;

                try
                {
                    using(FileStream fs = File.OpenRead(filename))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(Character));
                        Character loadCharacter = xs.Deserialize(fs) as Character;
                        _characters.Add(loadCharacter);
                        _lastCharacterPath = filename;
                      
                    }
                    CharactersListBox.ItemsSource = _characters;

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); 
                }
            }
        }

        private void OnSaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".char";

            if (_lastCharacterPath != null)
            {

              



                if (CharactersListBox.SelectedItem == null)
                {


                    MessageBox.Show("select a character to save");


                }
                else
                {

                    SaveCharacter(_lastCharacterPath);

                    MessageBox.Show($"The Character has been saved to{_lastCharacterPath}");
                    
                }

            }
            else
            {
                OnSaveAsCommandExecuted(sender, e);
            }
            
        }


        void SaveCharacter(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            




            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Character));
                xs.Serialize(fs, _characters[CharactersListBox.SelectedIndex]);


            }
            catch (SerializationException e)
            {
                MessageBox.Show(e.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            finally
            {
                fs.Close();
            }

          
            
        }


        private void OnSaveAsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".char";

            if (CharactersListBox.SelectedItem != null)
            {
                sfd.FileName = _characters[CharactersListBox.SelectedIndex].Name;

                bool? result = sfd.ShowDialog();
                if (result.HasValue)
                {
                    SaveCharacter(sfd.FileName);
                    _lastCharacterPath= sfd.FileName;

                }
                else
                {
                    MessageBox.Show("Saving Failed...");
                }
            }
            else
            {
                MessageBox.Show("Select a character to save");
            }

            
        }

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

  

        private void OnHelpButtonClick(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Owner= this;
            helpWindow.Title = "What can I do?";
            helpWindow.Show();
        }
    }
}
