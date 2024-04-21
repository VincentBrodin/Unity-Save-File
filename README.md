# Unity Save System

This code provides a simple async save system for Unity games. It allows you to create, load, and save data to a file within the persistent data path of your Unity project.

## Integration Guide

1. **Add the Code to Your Project:**
   - Copy the provided code into a new script file in your Unity project. You can name it something like "SaveSystem.cs".

2. **Integrate into any script**
   - You can now access the `SaveFile` class.
   - The `SaveFile` takes in 2 values, the desired name and the directories that it should lie in.
   - `SaveFile` handles the creation of any files or folders that it needs.

3. **Usage in Your Game:**
   - Use the `Set` method to save data. Provide a unique key, the value you want to save, and the data type (int, float, string, or bool).
   - Use the `Get` method to retrieve data using the key.
   - Call `WriteToFile` to save changes to the file.
   - Call `LoadFromFile` to load data from the file into the cache.
   - Use the appropriate events to set up functions when a file is done loading since the `SaveFile` is async.

4. **Accessing Saved Data:**
   - You can access saved data using the `Get` method, passing in the appropriate key.
   - Modify or retrieve data using the returned `Data` object.

5. **Unity Editor Integration (Optional):**
   - In Unity Editor, you can access a menu item under "Unity Tools" > "Save System" > "Open Persistent Data Path" to quickly navigate to the persistent data path directory where your save files are stored.

6. **Customization:**
   - You can customize the data types supported by adding more options to the `DataTypes` enum.
   - Extend the functionality as needed for your specific game requirements.

Remember to handle data loading and saving at appropriate points in your game, such as when the game starts, when the player progresses to a new level, or when they quit the game. Also, ensure proper error handling for file operations and data access to avoid issues during gameplay.😊
