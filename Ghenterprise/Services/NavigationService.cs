﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Ghenterprise.Services
{
    public class NavigationService
    {
        public event NavigatedEventHandler Navigated;
        public event NavigationFailedEventHandler NavigatedFailed;

        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();

        private Frame _frame;
        public Frame Frame
        {
            get
            {
                if (_frame == null)
                {
                    _frame = Window.Current.Content as Frame;
                    RegisterFrameEvents();
                }

                return _frame;
            }

            set
            {
                UnregisterFrameEvents();
                _frame = value;
                RegisterFrameEvents();
            }
        }

        private object _lastParamUsed;

        public bool CanGoBack => Frame.CanGoBack;
        public bool CanGoForward => Frame.CanGoForward;

        public bool GoBack()
        {
            if (CanGoBack)
            {
                Frame.GoBack();
                return true;
            }

            return false;
        }

        public void GoForward() => Frame.GoForward();

        private void Frame_NavigatedFailed(object sender, NavigationFailedEventArgs e) => NavigatedFailed?.Invoke(sender, e);
        private void Frame_Navigated(object sender, NavigationEventArgs e) => Navigated?.Invoke(sender, e);

        public bool Navigate(string pageKey, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            Type page;
            lock (_pages)
            {
                if (!_pages.TryGetValue(pageKey, out page))
                {
                    throw new ArgumentException(string.Format("Can't find page", pageKey), nameof(pageKey));
                }
            }

            //  Controleer of je niet naar de huidige pagina navigeert en of de meegegeven parameters kloppen
            if (Frame.Content?.GetType() != page || (parameter != null && !parameter.Equals(_lastParamUsed)))
            {
                var navigationResult = Frame.Navigate(page, parameter, infoOverride);
                if (navigationResult)
                {
                    _lastParamUsed = parameter;
                }

                return navigationResult;
            }
            else
            {
                return false;
            }
        }

        public void Configure(string key, Type pageType)
        {
            lock (_pages)
            {
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException("Key already exists!");
                }

                if (_pages.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException(string.Format("Page %s already exists!", _pages.First(p => p.Value == pageType).Key));
                }

                _pages.Add(key, pageType);
            }
        }

        public string GetNameOfRegisteredPage(Type page)
        {
            lock (_pages)
            {
                if (_pages.ContainsValue(page))
                {
                    return _pages.FirstOrDefault(p => p.Value == page).Key;
                }
                else
                {
                    throw new ArgumentException(string.Format("Page %s does not exist!", page.Name));
                }
            }
        }

        private void RegisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated += Frame_Navigated;
                _frame.NavigationFailed += Frame_NavigatedFailed;
            }
        }

        private void UnregisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated -= Frame_Navigated;
                _frame.NavigationFailed -= Frame_NavigatedFailed;
            }
        }
    }
}
