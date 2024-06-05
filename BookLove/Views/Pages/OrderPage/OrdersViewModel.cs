using BookLove.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BookLove.Views.Pages.OrderPage
{
    public class OrdersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> orders;
        private Order selectedOrder;

        public ObservableCollection<Order> Orders
        {
            get => orders;
            set
            {
                if (orders != value)
                {
                    orders = value;
                    OnPropertyChanged();
                }
            }
        }

        public Order SelectedOrder
        {
            get => selectedOrder;
            set
            {
                if (selectedOrder != value)
                {
                    selectedOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<OrderStatus> StatusOptions { get; set; }

        public OrdersViewModel()
        {
            LoadOrders();
            using (var db = new dbBookLoveEntities())
            {
                StatusOptions = new ObservableCollection<OrderStatus>(db.OrderStatus.ToList());
            }
        }

        private void LoadOrders()
        {
            using (var db = new dbBookLoveEntities())
            {
                Orders = new ObservableCollection<Order>(db.Order
                    .Include("OrderStatus")
                    .Include("OrderDetail.Book")
                    .Include("OrderDetail.Book.Genre")
                    .Include("OrderDetail.Book.Author")
                    .ToList());
            }
        }

        public void SaveChanges()
        {
            using (var db = new dbBookLoveEntities())
            {
                if (SelectedOrder != null)
                {
                    var orderToUpdate = db.Order.Include("OrderStatus").FirstOrDefault(o => o.id == SelectedOrder.id);
                    if (orderToUpdate != null)
                    {
                        var newStatus = db.OrderStatus.FirstOrDefault(os => os.id == SelectedOrder.OrderStatus.id);
                        if (newStatus != null)
                        {
                            orderToUpdate.statusId = newStatus.id;  // Обновление ID статуса
                            db.SaveChanges();
                            LoadOrders(); // Перезагрузка данных
                        }
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
