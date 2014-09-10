//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;

namespace AppBarUtils.TestApp.ViewModels
{
    public class FixedAppBarViewModel : ViewModelBase
    {
        #region initialization

        public FixedAppBarViewModel()
        {
            Data = new ObservableCollection<string>();

            SyncButtonDisplayText = "sync";

            Data.CollectionChanged +=
                (o, e) =>
                {
                    HasData = Data.Count > 0;
                    SyncButtonDisplayText = HasData ? "sync " + Data.Count : "sync";
                };

            _addCommands = new List<RelayCommand>
            {
                new RelayCommand(
                    () =>
                    {
                        Data.Insert(0, String.Format("hit {0} time(s)", Data.Count + 1));
                        RaisePropertyChanged("HitCount");
                    },
                    () => AddButtonIsEnabled),
                new RelayCommand(
                    () =>
                    {
                        Data.Insert(0, String.Format("hit at {0}", DateTime.Now.ToLongTimeString()));
                        RaisePropertyChanged("TimeCount");
                    },
                    () => AddButtonIsEnabled),
            };
            AddCommandDescriptions = new List<string>
            {
                "add hit count",
                "add current time",
            };

            AddCommand = _addCommands[0];
            ClearCommand = new RelayCommand(() => Data.Clear(), () => ClearMenuItemIsEnabled);

            LockCommand = new RelayCommand(() => IsLocked = true);
            UnlockCommand = new RelayCommand(() => IsLocked = false);
        }

        #endregion

        #region pivot control

        private int _selectedPivotItemIndex;
        public int SelectedPivotItemIndex
        {
            get { return _selectedPivotItemIndex; }
            set
            {
                if (_selectedPivotItemIndex != value)
                {
                    _selectedPivotItemIndex = value;
                    RaisePropertyChanged("SelectedPivotItemIndex");
                }
            }
        }

        #endregion

        #region data pivot item

        public ObservableCollection<string> Data { get; private set; }

        private bool _hasData;
        public bool HasData
        {
            get { return _hasData; }
            set
            {
                if (_hasData != value)
                {
                    _hasData = value;
                    RaisePropertyChanged("HasData");
                }
            }
        }

        #endregion

        #region query string parameters

        public string HitCount
        {
            get { return Data.Where(x => x.Contains("time(s)")).Count().ToString(); }
        }

        public string TimeCount
        {
            get { return Data.Where(x => x.Contains("at")).Count().ToString(); }
        }

        #endregion

        #region settings pivot item

        private IList<RelayCommand> _addCommands;
        public IList<string> AddCommandDescriptions { get; private set; }

        private int _selectedAddCommandIndex;
        public int SelectedAddCommandIndex
        {
            get { return _selectedAddCommandIndex; }
            set
            {
                if (_selectedAddCommandIndex != value)
                {
                    _selectedAddCommandIndex = value;
                    AddCommand = _addCommands[_selectedAddCommandIndex];
                    RaisePropertyChanged("SelectedAddCommandIndex");
                }
            }
        }

        private bool _addButtonIsEnabled;
        public bool AddButtonIsEnabled
        {
            get { return _addButtonIsEnabled; }
            set
            {
                if (_addButtonIsEnabled != value)
                {
                    _addButtonIsEnabled = value;
                    AddCommand.RaiseCanExecuteChanged();
                    RaisePropertyChanged("AddButtonIsEnabled");
                }
            }
        }

        private bool _clearMenuItemIsEnabled;
        public bool ClearMenuItemIsEnabled
        {
            get { return _clearMenuItemIsEnabled; }
            set
            {
                if (_clearMenuItemIsEnabled != value)
                {
                    _clearMenuItemIsEnabled = value;
                    ClearCommand.RaiseCanExecuteChanged();
                    RaisePropertyChanged("ClearMenuItemIsEnabled");
                }
            }
        }

        #endregion

        #region application bar commands

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get { return _addCommand; }
            set
            {
                if (_addCommand != value)
                {
                    _addCommand = value;
                    RaisePropertyChanged("AddCommand");
                }
            }
        }

        private RelayCommand _clearCommand;
        public RelayCommand ClearCommand
        {
            get { return _clearCommand; }
            set
            {
                if (_clearCommand != value)
                {
                    _clearCommand = value;
                    RaisePropertyChanged("ClearCommand");
                }
            }
        }

        private RelayCommand _lockCommand;
        public RelayCommand LockCommand
        {
            get { return _lockCommand; }
            set
            {
                if (_lockCommand != value)
                {
                    _lockCommand = value;
                    RaisePropertyChanged("LockCommand");
                }
            }
        }

        private RelayCommand _unlockCommand;
        public RelayCommand UnlockCommand
        {
            get { return _unlockCommand; }
            set
            {
                if (_unlockCommand != value)
                {
                    _unlockCommand = value;
                    RaisePropertyChanged("UnlockCommand");
                }
            }
        }

        public void Sync()
        {
            System.Windows.MessageBox.Show("Sync completed.");
        }

        #endregion

        #region application bar texts

        private string _syncButtonDisplayText;
        public string SyncButtonDisplayText
        {
            get { return _syncButtonDisplayText; }
            set
            {
                if (_syncButtonDisplayText != value)
                {
                    _syncButtonDisplayText = value;
                    RaisePropertyChanged("SyncButtonDisplayText");
                }
            }
        }

        private string _lockButtonDisplayText;
        public string LockButtonDisplayText
        {
            get { return _lockButtonDisplayText; }
            set
            {
                if (_lockButtonDisplayText != value)
                {
                    _lockButtonDisplayText = value;
                    RaisePropertyChanged("LockButtonDisplayText");
                }
            }
        }

        #endregion

        private bool _isLocked = false;

        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (_isLocked != value)
                {
                    _isLocked = value;
                    RaisePropertyChanged("IsLocked");
                }
            }
        }
    }
}
