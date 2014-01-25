Sitecore Content Usage Tools
============================

## Synopsis

Tools for visualizing **content dependencies** within Sitecore; when editing content, see on which pages that content is being used.

## Motivation

When editing content, it is of vital importance that the editor knows exactly what he/she is changing. In other words; what **impact** do specific content changes have on the actual website content.

> A common pitfall is when content items are set as a datasource for components on different pages and the editor changes the value of one of the fields from the page editor. Changing the content on the other pages may be desired, but perhaps not always. We aim to make it more clear to content editors, what exact pages will be affected when changing content (even in the page editor).

## Features

* In the **content editor**, there is a gutter item that can be used to spot content items that are not being used in any page (they can be deleted by clicking on them).
* In the **content editor**, a message is displayed above the fields to indicate on what pages specific the item is being used.
* In the **page editor**, a floating pane is displayed when a component is selected. The pane displays the pages on which the content is being used.
* From the **desktop start menu**, select "Reporting Tools" > "Content Usage Report" to get detailed reports about where content is being used and what content is not being used anywhere. This can be powered by the Sitecore 7 search indexes or the regular (slower) way.

## Installation

Download the latest installation package from this link.

## Contributors

* **Dražen Janjiček** - @djanjicek
* **Johann Baziret** - @bazijjoba
* **Robin Hermanussen** - @knifecore

## License

Copyright (C) 2014 Dražen Janjiček, Johann Baziret, Robin Hermanussen

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <http://www.gnu.org/licenses/>.