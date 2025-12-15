ITD 

Overview:
This module is an Augmented Reality promotional game for Thye Shan Medical. Players interact with Chinese herbs and craft traditional medicines in an AR environment. The game is meant to showcase AR technology while providing a fun and educational experience.

Platforms & Hardware Requirements:
The game runs on Android devices that support ARCore. Minimum requirements include Android 10 or higher, a camera, 2GB of RAM, 720p screen resolution, and at least 100MB of free storage.

Controls and Gameplay:
Players move physically in AR space to scan markers that spawn cabinets (herbs), crafting tables, and customers. Tap objects to interact.
Cabinets let you pick herbs, but inventory is limited to 2 herbs at a time.
Crafting tables let you combine two herbs to create a medicine.
Customers appear in front of the table. Tap a customer to see their symptoms, then give them the correct medicine.
UI buttons are tapped to navigate menus, start the game, and access the recipe panel. Email and password typing is required for login and signup. No cheats or hacks are implemented.

Gameplay Flow:
Start with login or signup. Scan the AR space to spawn objects. Collect up to 2 herbs. Use the crafting table to make medicine. Serve two customers correctly to finish the round. After both customers are served, the player earns 10 reputation points. The reputation points are displayed permanently, and the recipe can be accessed anytime via the menu.

Inventory and Medicine System:
Inventory holds a maximum of 2 herbs. The herbs and their descriptions are:
Ginseng Root: Enhances vitality, stamina, and combats weakness.
Astragalus Root: Supports energy, strengthens the body, aids recovery.
Tortoise Plastron: Cools internal heat and restores balance.
Selfheal Spike: Reduces internal heat and relieves inflammation.
Water Buffalo Horn: Regulates temperature and counters cold or fever.
Fresh Ginger Rhizome: Dispels cold, improves circulation, relieves chills.
Prepared Rehmannia Root: Nourishes kidneys and restores balance.
Cornelian Cherry Fruit: Supports kidney function and stabilizes vital essence.

The medicines are:
Vitality Tonic Pill: Ginseng Root + Astragalus Root, gold colored.
Heat Relief Herbal Pill: Tortoise Plastron + Selfheal Spike, blue colored.
Cold Relief Herbal Powder: Water Buffalo Horn + Fresh Ginger Rhizome, red colored.
Kidney Nourishing Herbal Pill: Prepared Rehmannia Root + Cornelian Cherry Fruit, gray colored.

Known Bugs and Limitations:
Only one level is implemented. AR experience depends on the device’s ARCore support. Inventory is limited to two herbs.

References and Credits:
All models, textures, and materials were hand-crafted by the development team. The AR system is built with Unity AR Foundation and the UI uses TextMeshPro.

Solutions / Answer Key:
Ginseng Root + Astragalus Root → Vitality Tonic Pill
Tortoise Plastron + Selfheal Spike → Heat Relief Herbal Pill
Water Buffalo Horn + Fresh Ginger Rhizome → Cold Relief Herbal Powder
Prepared Rehmannia Root + Cornelian Cherry Fruit → Kidney Nourishing Herbal Pill

Wireframes / Game Flow:
Start → Login/Signup → Main Menu → Scan AR Space → Pick Herbs → Craft Medicine → Serve Customer → End Game → Reputation UI
Recipe UI is accessible anytime via the menu, and reputation is visible permanently.

DDA 

Overview:
This module handles backend services using Firebase, including authentication and real-time database. It manages user accounts, stores reputation points, and ensures data persistence across sessions.

Platforms & Hardware Requirements:
Requires Unity 2021 or higher, Android/iOS build support, and internet access for Firebase services.

Features:
Authentication allows users to sign up and login using email and password, with proper error handling.
Realtime Database stores user ID and reputation points and ensures data is secure and persistent.
UI Integration connects the login, signup, start, main, and end-game panels with the backend. Reputation points are displayed permanently.

Gameplay Integration:
Reputation points are updated after serving customers in the AR module. Database rules ensure unique user IDs and valid data writes.

Known Bugs and Limitations:
Internet connection is required to login/signup and save reputation. No web frontend for administration has been implemented.

References and Credits:
Firebase Unity SDK is used for Authentication and Realtime Database. UI uses Unity UI system and TextMeshPro.

Wireframes / Game Flow:
Login/Signup → Validate → Save user ID in Firebase → Main Panel → Play AR Game → Update Reputation in Firebase → End Game

