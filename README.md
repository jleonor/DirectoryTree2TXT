# Directory Tree Viewer

Directory Tree Viewer is a WPF application that generates a textual representation of your directory structure. With its intuitive interface and powerful features, you can easily browse, filter, and save the output of your directory trees.

---

## Features

- **Recursive Tree Generation**  
  Traverse directories recursively and display a clean, indented tree structure of folders and files.

- **Exclusion Rules with Wildcard Support**  
  Define exclusion entries using wildcards to either **hide** or **ignore** specific files or folders:  
  - **Hide:** Completely removes a file or folder from the tree.  
  - **Ignore:** Displays the file or folder name but omits its contents. This means that even if the "Show File Contents" option is enabled, files or folders marked as ignored will not show their contents.

- **File Content Preview**  
  Optionally include the contents of files directly within the tree output for a detailed view.

- **Configuration Management**  
  Save, load, rename, and delete search configurations so you can quickly repeat your favorite directory scans.

- **Custom UI Components**  
  Enjoy a modern and responsive UI with a custom title bar, scrollbars, and themed styles—all built using XAML and C#.

---

## Project Structure

```
/DirectoryTree2TXT
├── /Controls                # Custom UI controls (e.g., custom title bar)
├── /Converters              # Value converters for data binding
├── /Helpers                 # Command helpers (RelayCommand)
├── /Models                  # Data models for exclusion entries and search configuration
├── /Resources               # Custom styles, themes, and scroll bar templates
├── /Services                # Core logic for generating directory trees and managing configurations
├── /ViewModels              # MVVM view models handling the application logic
└── /Views                   # XAML views for the user interface
```

---

## Prerequisites

- **.NET Framework / .NET Core/5+:** Ensure you have a version of .NET that supports WPF.
- **Visual Studio:** Recommended for building and debugging the project.

---

## Getting Started

1. **Clone the Repository**

   ```bash
   git clone https://github.com/jleonor/DirectoryTree2TXT.git
   ```

2. **Open the Solution**

   Open the solution file (`.sln`) in Visual Studio.

3. **Build and Run**

   Build the solution and run the application. You will see the main window with options to browse for a directory, add exclusion rules, and generate the directory tree.

4. **Usage**

   - **Browse for a Directory:**  
     Click the **Browse** button to select the folder you want to scan.
     
   - **Configure Exclusions:**  
     Add entries (with support for wildcards) to hide or ignore certain files or directories.
     
   - **Generate the Tree:**  
     Click **Generate Tree** to view the directory structure. Optionally, toggle the option to include file contents.
     
   - **Save Configurations:**  
     Save your current settings as a configuration file for future use. Manage your configurations through the interface with options to rename or delete them.

---

## Customization

- **UI Customization:**  
  Modify the XAML files in the `/Resources` folder (e.g., `Styles.xaml`, `Theme.xaml`, and `CustomScrollBar.xaml`) to change the appearance of the application.

- **Logic Adjustments:**  
  Update the directory traversal logic in `DirectoryTreeGenerator.cs` or adjust exclusion matching in the same file to suit your needs.

- **Extending Functionality:**  
  The project follows the MVVM pattern. Use and extend the ViewModels in the `/ViewModels` folder to add new features or improve the existing functionality.

---

## Contributing

Contributions are welcome! If you have suggestions, bug fixes, or improvements, please follow these steps:

1. Fork the repository.
2. Create a new branch with your changes.
3. Submit a pull request detailing your changes and the benefits.

---

## License

Distributed under the GNU GENERAL PUBLIC License. See the [LICENSE](LICENSE.txt) file for more information.

---

## Contact

For questions, suggestions, or feedback, please open an issue on GitHub or contact the project maintainer at [jonathanleonor@gmail.com](mailto:jonathanleonor@gmail.com).
