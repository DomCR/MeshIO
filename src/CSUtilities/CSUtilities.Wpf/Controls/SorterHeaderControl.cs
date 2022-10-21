using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CSUtilities.Wpf.Controls
{
	public class SorterHeaderControl : GridViewColumnHeader
	{
		private GridViewColumnHeader m_lastHeaderClicked = null;
		private ListSortDirection m_lastDirection = ListSortDirection.Ascending;

		public SorterHeaderControl()
		{
			Click += reorderList;
		}
		//**************************************************************************************
		private void reorderList(object sender, System.Windows.RoutedEventArgs e)
		{
			GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
			ListSortDirection direction;

			if (headerClicked != null)
			{
				if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
				{
					if (headerClicked != m_lastHeaderClicked)
					{
						direction = ListSortDirection.Ascending;
					}
					else
					{
						if (m_lastDirection == ListSortDirection.Ascending)
						{
							direction = ListSortDirection.Descending;
						}
						else
						{
							direction = ListSortDirection.Ascending;
						}
					}

					var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
					var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

					sort(sortBy, direction, sender as ListView);

					if (direction == ListSortDirection.Ascending)
					{
						headerClicked.Column.HeaderTemplate =
						  Resources["HeaderTemplateArrowUp"] as DataTemplate;
					}
					else
					{
						headerClicked.Column.HeaderTemplate =
						  Resources["HeaderTemplateArrowDown"] as DataTemplate;
					}

					// Remove arrow from previously sorted header
					if (m_lastHeaderClicked != null && m_lastHeaderClicked != headerClicked)
					{
						m_lastHeaderClicked.Column.HeaderTemplate = null;
					}

					m_lastHeaderClicked = headerClicked;
					m_lastDirection = direction;
				}
			}
		}
		private void sort(string sortBy, ListSortDirection direction, ListView lv)
		{
			ICollectionView dataView =
			  CollectionViewSource.GetDefaultView(lv.ItemsSource);

			dataView.SortDescriptions.Clear();
			SortDescription sd = new SortDescription(sortBy, direction);
			dataView.SortDescriptions.Add(sd);
			dataView.Refresh();
		}
	}
}
