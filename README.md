# FullCalendar for nopCommerce 4.5

FullCalendar is a powerful and easy-to-use nopCommerce plugin that integrates Google Calendar with FullCalendar.io, providing a seamless appointment and booking system for your nopCommerce 4.5 website. This plugin is an upgraded version of a previous plugin designed for nopCommerce 3.8, now enhanced to work with the latest nopCommerce features. It's a straightforward tool designed for community contribution and growth, and it's open for anyone to use and extend.

## Features
- Google Calendar integration for managing appointments and bookings.
- FullCalendar.io integration for a rich and interactive calendar experience.
- Easy configuration and setup through the nopCommerce admin panel.
- Open source and community-driven for continuous improvements.

## Installation
To install the FullCalendar plugin, follow these steps:

### Downloading the Plugin
Download the `nop.plugin.calendar` folder from the GitHub repository.

### Installation via nopCommerce Admin Panel
1. Log in to your nopCommerce admin panel.
2. Navigate to the plugin section.
3. Upload the plugin manually by selecting the downloaded folder.

### Installation via Visual Studio or Any IDE
1. Extract the zip file of the plugin.
2. Add the extracted solution under the `plugins` directory of your nopCommerce solution.
3. Ensure the plugin folder is also copied to `nopCommerce/plugins` in your project directory.

> Note: If you require the released version of the plugin, please contact me. If there is sufficient interest, I will share a pre-compiled version to simplify the installation process.

4. Build the solution in your IDE.

> Note: Verify that the plugin appears under `nopCommerce/presentation/plugins`. The folder should contain `.dll` files.

> Note: Ensure all files in `Views` and `plugin.json` are set to 'Content' in the file properties.

## Configuration and Usage
Once installed, you need to configure and enable the plugin:

1. Enable the plugin through the nopCommerce admin panel by navigating to the plugin list and using the 'Edit' option.
2. Go to the plugin configuration page and enter your Google Calendar ID. Instructions for obtaining your Google Calendar ID can be found with a simple Google search. There is no need to change any other settings unless necessary for your specific use case.
3. On the configuration page, set the availability dates for your calendar and save the changes.

That's it! Enjoy your new FullCalendar integration.

## Contributing
We welcome contributions from the community! Whether you're fixing bugs, adding new features, or improving documentation, your help is appreciated. Please feel free to fork the repository, make your changes, and submit a pull request.

## Support
If you encounter any issues or have questions regarding the FullCalendar plugin, please open an issue on the GitHub repository, and we'll do our best to assist you.

## License
This plugin is open-source and free to use under the MIT License.

Thank you for choosing FullCalendar for your nopCommerce website. We hope it enhances your customers' experience and streamlines your appointment and booking management.
