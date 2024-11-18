# unity-ar-sample

A small AR Core + Extensions + Niantic Lightship example
* Define some coorinates for objects
* Throw balls at them


## Before build
May need to install AR Core Extensions targall from https://github.com/google-ar/arcore-unity-extensions/releases `*arf5` specifically!!!
It is also provided in `Packages` folder
Git is required to be installed and set in Path to resolve git dependencies

Pre-defined anchors can be set in `Assets/Settings/PreDefinedAnchors` scriptable object, add any amount of entries you want. Only Latitude and Logintude are required, rest are optional

## Build for Android
* `Assets/External Dependency Manager/Android Resolver/Resolve`
* Put your Google Cloud project API key in `Edit/Project Settings/XR Plug-in Management/ARCore Extensions`, `Android API Key` if you want to use API authentication
* Other way you can sign android application with keysotre and specify its SHA-1 in OAuth Client in Google Cloud Project
* Put you Lightship API key in `Lightship/Settings/API Key`
* Click `Build and Run`

## Build for iOS

Unfortunately I can't test all this stuff but you still
* Put your Google or IOS:] Cloud project API key in `Edit/Project Settings/XR Plug-in Management/ARCore Extensions`, `iOS API Key` if you want to use API authentication
* I'm not sure if SHA-1 is available on iOS
* Put you Lightship API key in `Lightship/Settings/API Key`
* Click `Build and Run`
* And then something in XCode

### Plugins used
* [Zenject](https://github.com/modesttree/Zenject) Flexible DI
* [UniTask](https://github.com/Cysharp/UniTask) Cool async tool for Unity
* [Niantic Lightship](https://lightship.dev/) Ground detection was used from this library
* [AR Foundation](https://developers.google.com/ar)
* [ARCore Extensions](https://github.com/google-ar/arcore-unity-extensions) Geospatial features

### External links
* [Cool samples](https://github.com/TakashiYoshinaga/GeospatialAPI-Unity-StarterKit)
