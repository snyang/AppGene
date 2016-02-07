using AppGene.Db.Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppGene.Model.Entities
{
    public partial class Employee : IUiModel
    {
        private Memento<Employee> memento;

        private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        #region INotifyPropertyChanged Members

        private event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                if (this is INotifyPropertyChanged)
                {
                    this.PropertyChanged += value;
                    //((INotifyPropertyChanged)this).PropertyChanged +=
                    //                              this.NotifyPropertyChanged;
                }
            }

            remove
            {
                if (this is INotifyPropertyChanged)
                {
                    this.PropertyChanged -= value;
                    //((INotifyPropertyChanged)this).PropertyChanged -=
                    //                             this.NotifyPropertyChanged;
                }
            }
        }

        #endregion

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                return this.ValidateColumn(columnName);
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                return this.ValidateObject();
            }
        }

        bool IUiModel.TraceChanges { get; set; }

        bool IUiModel.IsChanged { get; set; }

        bool IUiModel.IsNew
        {
            get
            {
                return this.EmployeeID == 0;
            }
        }

        void IEditableObject.BeginEdit()
        {
            if (this.memento == null)
            {
                this.memento = new Memento<Employee>(this);
                (this as IUiModel).TraceChanges = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (this.memento != null)
            {
                (this as IUiModel).TraceChanges = false;
                this.memento.Restore(this);
                foreach (var pair in this.memento.StoredProperties)
                {
                    this.NotifyPropertyChanged(this, new PropertyChangedEventArgs(pair.Key.Name));
                }

                this.memento = null;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (this.memento != null)
            {
                (this as IUiModel).TraceChanges = false;
                this.memento = null;
            }
        }

        void HandleSet(Action setAction, [CallerMemberName] String propertyName = null)
        {
            setAction.Invoke();
            this.NotifyPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            if ((this as IUiModel).TraceChanges) (this as IUiModel).IsChanged = true;
        }

        bool IUiModel.DoFilter(string filterString)
        {
            if (this.EmployeeName.IndexOf(filterString, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (this.EmployeeCode.ToString().Contains(filterString)) return true;

            return false;
        }

        void IUiModel.SetDefault()
        {
            this.EmployeeName = "New Employee";
        }

        string IUiModel.ToDisplayString()
        {
            return string.Format("{0:000000} - {1}", employeeCode, employeeName);
        }
    }
}
