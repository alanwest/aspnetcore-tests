| | Scenario | Request | Activity.DisplayName | Expected http.route | Strategy 1 | Strategy 2 | Strategy 3 |
| - | - | - | - | - | - | - | - |
| [1](#1) | ConventionalRouting | GET / | / | ConventionalRoute/Default/{id?} | {controller=ConventionalRoute}/{action=Default}/{id?} | ConventionalRoute/Default/{id?} | ConventionalRoute/Default |
| [2](#2) | ConventionalRouting | GET /ConventionalRoute/ActionWithStringParameter/2?num=3 | /ConventionalRoute/ActionWithStringParameter/2 | ConventionalRoute/ActionWithStringParameter/{id?} | {controller=ConventionalRoute}/{action=Default}/{id?} | ConventionalRoute/ActionWithStringParameter/{id?} | ConventionalRoute/ActionWithStringParameter/{id}/{num} |
| [3](#3) | ConventionalRouting | GET /ConventionalRoute/ActionWithStringParameter?num=3 | /ConventionalRoute/ActionWithStringParameter | ConventionalRoute/ActionWithStringParameter/{id?} | {controller=ConventionalRoute}/{action=Default}/{id?} | ConventionalRoute/ActionWithStringParameter/{id?} | ConventionalRoute/ActionWithStringParameter/{id}/{num} |
| [4](#4) | ConventionalRouting | GET /ConventionalRoute/NotFound | /ConventionalRoute/NotFound |  |  |  |  |
| [5](#5) | ConventionalRouting | GET /SomePath/SomeString/2 | /SomePath/SomeString/2 | SomePath/{id}/{num:int} | SomePath/{id}/{num:int} | SomePath/{id}/{num:int} | ConventionalRoute/ActionWithStringParameter/{id}/{num} |
| [6](#6) | ConventionalRouting | GET /SomePath/SomeString/NotAnInt | /SomePath/SomeString/NotAnInt |  |  |  |  |
| [7](#7) | ConventionalRouting | GET /MyArea | /MyArea |  | {area:exists}/{controller=MyArea}/{action=Default}/{id?} | {area:exists}/MyArea/Default/{id?} | MyArea/Default |
| [8](#8) | ConventionalRouting | GET /Dude | /Dude |  | Dude/{controller=AnotherArea}/{action=Index}/{id?} | Dude/AnotherArea/Index/{id?} | AnotherArea/Index |
| [9](#9) | AttributeRouting | GET /AttributeRoute | AttributeRoute | AttributeRoute | AttributeRoute | AttributeRoute | AttributeRoute/Get |
| [10](#10) | AttributeRouting | GET /AttributeRoute/Get | AttributeRoute/Get | AttributeRoute/Get | AttributeRoute/Get | AttributeRoute/Get | AttributeRoute/Get |
| [11](#11) | AttributeRouting | GET /AttributeRoute/Get/12 | AttributeRoute/Get/{id} | AttributeRoute/Get/{id?} | AttributeRoute/Get/{id} | AttributeRoute/Get/{id} | AttributeRoute/Get/{id} |
| [12](#12) | AttributeRouting | GET /AttributeRoute/12/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/GetWithActionNameInDifferentSpotInTemplate/{id} |
| [13](#13) | AttributeRouting | GET /AttributeRoute/NotAnInt/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/GetWithActionNameInDifferentSpotInTemplate/{id} |
| [14](#14) | RazorPages | GET / | / | /Index |  | /Index |  |
| [15](#15) | RazorPages | GET /Index | Index | /Index | Index | /Index |  |
| [16](#16) | RazorPages | GET /PageThatThrowsException | PageThatThrowsException | /PageThatThrowsException | PageThatThrowsException | /PageThatThrowsException |  |
| [17](#17) | RazorPages | GET /js/site.js | /js/site.js |  |  |  |  |
| [18](#18) | MinimalApi | GET /MinimalApi | /MinimalApi |  |  |  |  |
| [19](#19) | MinimalApi | GET /MinimalApi/123 | /MinimalApi/123 |  |  |  |  |

#### 1

```json
{
  "HttpMethod": "GET",
  "Path": "/",
  "HttpRouteByRawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "HttpRouteByControllerActionAndParameters": "ConventionalRoute/Default",
  "HttpRouteByActionDescriptor": "ConventionalRoute/Default/{id?}",
  "DebugInfo": {
    "RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "RouteDiagnosticMetadata": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "RouteData": {
      "controller": "ConventionalRoute",
      "action": "Default"
    },
    "AttributeRouteInfo": null,
    "ActionParameters": [],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "ConventionalRoute",
    "ControllerActionDescriptorActionName": "Default"
  }
}
```

#### 2

```json
{
  "HttpMethod": "GET",
  "Path": "/ConventionalRoute/ActionWithStringParameter/2?num=3",
  "HttpRouteByRawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "HttpRouteByControllerActionAndParameters": "ConventionalRoute/ActionWithStringParameter/{id}/{num}",
  "HttpRouteByActionDescriptor": "ConventionalRoute/ActionWithStringParameter/{id?}",
  "DebugInfo": {
    "RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "RouteDiagnosticMetadata": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "RouteData": {
      "controller": "ConventionalRoute",
      "action": "ActionWithStringParameter",
      "id": "2"
    },
    "AttributeRouteInfo": null,
    "ActionParameters": [
      "id",
      "num"
    ],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "ConventionalRoute",
    "ControllerActionDescriptorActionName": "ActionWithStringParameter"
  }
}
```

#### 3

```json
{
  "HttpMethod": "GET",
  "Path": "/ConventionalRoute/ActionWithStringParameter?num=3",
  "HttpRouteByRawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "HttpRouteByControllerActionAndParameters": "ConventionalRoute/ActionWithStringParameter/{id}/{num}",
  "HttpRouteByActionDescriptor": "ConventionalRoute/ActionWithStringParameter/{id?}",
  "DebugInfo": {
    "RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "RouteDiagnosticMetadata": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "RouteData": {
      "controller": "ConventionalRoute",
      "action": "ActionWithStringParameter"
    },
    "AttributeRouteInfo": null,
    "ActionParameters": [
      "id",
      "num"
    ],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "ConventionalRoute",
    "ControllerActionDescriptorActionName": "ActionWithStringParameter"
  }
}
```

#### 4

```json
{
  "HttpMethod": "GET",
  "Path": "/ConventionalRoute/NotFound",
  "HttpRouteByRawText": null,
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": "",
  "DebugInfo": {
    "RawText": null,
    "RouteDiagnosticMetadata": null,
    "RouteData": {},
    "AttributeRouteInfo": null,
    "ActionParameters": null,
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": null,
    "ControllerActionDescriptorActionName": null
  }
}
```

#### 5

```json
{
  "HttpMethod": "GET",
  "Path": "/SomePath/SomeString/2",
  "HttpRouteByRawText": "SomePath/{id}/{num:int}",
  "HttpRouteByControllerActionAndParameters": "ConventionalRoute/ActionWithStringParameter/{id}/{num}",
  "HttpRouteByActionDescriptor": "SomePath/{id}/{num:int}",
  "DebugInfo": {
    "RawText": "SomePath/{id}/{num:int}",
    "RouteDiagnosticMetadata": "SomePath/{id}/{num:int}",
    "RouteData": {
      "controller": "ConventionalRoute",
      "action": "ActionWithStringParameter",
      "id": "SomeString",
      "num": "2"
    },
    "AttributeRouteInfo": null,
    "ActionParameters": [
      "id",
      "num"
    ],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "ConventionalRoute",
    "ControllerActionDescriptorActionName": "ActionWithStringParameter"
  }
}
```

#### 6

```json
{
  "HttpMethod": "GET",
  "Path": "/SomePath/SomeString/NotAnInt",
  "HttpRouteByRawText": null,
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": "",
  "DebugInfo": {
    "RawText": null,
    "RouteDiagnosticMetadata": null,
    "RouteData": {},
    "AttributeRouteInfo": null,
    "ActionParameters": null,
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": null,
    "ControllerActionDescriptorActionName": null
  }
}
```

#### 7

```json
{
  "HttpMethod": "GET",
  "Path": "/MyArea",
  "HttpRouteByRawText": "{area:exists}/{controller=MyArea}/{action=Default}/{id?}",
  "HttpRouteByControllerActionAndParameters": "MyArea/Default",
  "HttpRouteByActionDescriptor": "{area:exists}/MyArea/Default/{id?}",
  "DebugInfo": {
    "RawText": "{area:exists}/{controller=MyArea}/{action=Default}/{id?}",
    "RouteDiagnosticMetadata": "{area:exists}/{controller=MyArea}/{action=Default}/{id?}",
    "RouteData": {
      "controller": "MyArea",
      "action": "Default",
      "area": "MyArea"
    },
    "AttributeRouteInfo": null,
    "ActionParameters": [],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "MyArea",
    "ControllerActionDescriptorActionName": "Default"
  }
}
```

#### 8

```json
{
  "HttpMethod": "GET",
  "Path": "/Dude",
  "HttpRouteByRawText": "Dude/{controller=AnotherArea}/{action=Index}/{id?}",
  "HttpRouteByControllerActionAndParameters": "AnotherArea/Index",
  "HttpRouteByActionDescriptor": "Dude/AnotherArea/Index/{id?}",
  "DebugInfo": {
    "RawText": "Dude/{controller=AnotherArea}/{action=Index}/{id?}",
    "RouteDiagnosticMetadata": "Dude/{controller=AnotherArea}/{action=Index}/{id?}",
    "RouteData": {
      "area": "AnotherArea",
      "controller": "AnotherArea",
      "action": "Index"
    },
    "AttributeRouteInfo": null,
    "ActionParameters": [],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "AnotherArea",
    "ControllerActionDescriptorActionName": "Index"
  }
}
```

#### 9

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute",
  "HttpRouteByRawText": "AttributeRoute",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/Get",
  "HttpRouteByActionDescriptor": "AttributeRoute",
  "DebugInfo": {
    "RawText": "AttributeRoute",
    "RouteDiagnosticMetadata": "AttributeRoute",
    "RouteData": {
      "action": "Get",
      "controller": "AttributeRoute"
    },
    "AttributeRouteInfo": "AttributeRoute",
    "ActionParameters": [],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "AttributeRoute",
    "ControllerActionDescriptorActionName": "Get"
  }
}
```

#### 10

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute/Get",
  "HttpRouteByRawText": "AttributeRoute/Get",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/Get",
  "HttpRouteByActionDescriptor": "AttributeRoute/Get",
  "DebugInfo": {
    "RawText": "AttributeRoute/Get",
    "RouteDiagnosticMetadata": "AttributeRoute/Get",
    "RouteData": {
      "action": "Get",
      "controller": "AttributeRoute"
    },
    "AttributeRouteInfo": "AttributeRoute/Get",
    "ActionParameters": [],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "AttributeRoute",
    "ControllerActionDescriptorActionName": "Get"
  }
}
```

#### 11

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute/Get/12",
  "HttpRouteByRawText": "AttributeRoute/Get/{id}",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/Get/{id}",
  "HttpRouteByActionDescriptor": "AttributeRoute/Get/{id}",
  "DebugInfo": {
    "RawText": "AttributeRoute/Get/{id}",
    "RouteDiagnosticMetadata": "AttributeRoute/Get/{id}",
    "RouteData": {
      "action": "Get",
      "controller": "AttributeRoute",
      "id": "12"
    },
    "AttributeRouteInfo": "AttributeRoute/Get/{id}",
    "ActionParameters": [
      "id"
    ],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "AttributeRoute",
    "ControllerActionDescriptorActionName": "Get"
  }
}
```

#### 12

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute/12/GetWithActionNameInDifferentSpotInTemplate",
  "HttpRouteByRawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/GetWithActionNameInDifferentSpotInTemplate/{id}",
  "HttpRouteByActionDescriptor": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "DebugInfo": {
    "RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "RouteDiagnosticMetadata": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "RouteData": {
      "action": "GetWithActionNameInDifferentSpotInTemplate",
      "controller": "AttributeRoute",
      "id": "12"
    },
    "AttributeRouteInfo": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "ActionParameters": [
      "id"
    ],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "AttributeRoute",
    "ControllerActionDescriptorActionName": "GetWithActionNameInDifferentSpotInTemplate"
  }
}
```

#### 13

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute/NotAnInt/GetWithActionNameInDifferentSpotInTemplate",
  "HttpRouteByRawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/GetWithActionNameInDifferentSpotInTemplate/{id}",
  "HttpRouteByActionDescriptor": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "DebugInfo": {
    "RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "RouteDiagnosticMetadata": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "RouteData": {
      "action": "GetWithActionNameInDifferentSpotInTemplate",
      "controller": "AttributeRoute",
      "id": "NotAnInt"
    },
    "AttributeRouteInfo": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "ActionParameters": [
      "id"
    ],
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": "AttributeRoute",
    "ControllerActionDescriptorActionName": "GetWithActionNameInDifferentSpotInTemplate"
  }
}
```

#### 14

```json
{
  "HttpMethod": "GET",
  "Path": "/",
  "HttpRouteByRawText": "",
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": "/Index",
  "DebugInfo": {
    "RawText": "",
    "RouteDiagnosticMetadata": "",
    "RouteData": {
      "page": "/Index"
    },
    "AttributeRouteInfo": "",
    "ActionParameters": [],
    "PageActionDescriptorRelativePath": "/Pages/Index.cshtml",
    "PageActionDescriptorViewEnginePath": "/Index",
    "ControllerActionDescriptorControllerName": null,
    "ControllerActionDescriptorActionName": null
  }
}
```

#### 15

```json
{
  "HttpMethod": "GET",
  "Path": "/Index",
  "HttpRouteByRawText": "Index",
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": "/Index",
  "DebugInfo": {
    "RawText": "Index",
    "RouteDiagnosticMetadata": "Index",
    "RouteData": {
      "page": "/Index"
    },
    "AttributeRouteInfo": "Index",
    "ActionParameters": [],
    "PageActionDescriptorRelativePath": "/Pages/Index.cshtml",
    "PageActionDescriptorViewEnginePath": "/Index",
    "ControllerActionDescriptorControllerName": null,
    "ControllerActionDescriptorActionName": null
  }
}
```

#### 16

```json
{
  "HttpMethod": "GET",
  "Path": "/PageThatThrowsException",
  "HttpRouteByRawText": "PageThatThrowsException",
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": "/PageThatThrowsException",
  "DebugInfo": {
    "RawText": "PageThatThrowsException",
    "RouteDiagnosticMetadata": "PageThatThrowsException",
    "RouteData": {
      "page": "/PageThatThrowsException"
    },
    "AttributeRouteInfo": "PageThatThrowsException",
    "ActionParameters": [],
    "PageActionDescriptorRelativePath": "/Pages/PageThatThrowsException.cshtml",
    "PageActionDescriptorViewEnginePath": "/PageThatThrowsException",
    "ControllerActionDescriptorControllerName": null,
    "ControllerActionDescriptorActionName": null
  }
}
```

#### 17

```json
{
  "HttpMethod": "GET",
  "Path": "/js/site.js",
  "HttpRouteByRawText": null,
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": "",
  "DebugInfo": {
    "RawText": null,
    "RouteDiagnosticMetadata": null,
    "RouteData": {},
    "AttributeRouteInfo": null,
    "ActionParameters": null,
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": null,
    "ControllerActionDescriptorActionName": null
  }
}
```

#### 18

```json
{
  "HttpMethod": "GET",
  "Path": "/MinimalApi",
  "HttpRouteByRawText": null,
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": "",
  "DebugInfo": {
    "RawText": null,
    "RouteDiagnosticMetadata": null,
    "RouteData": {},
    "AttributeRouteInfo": null,
    "ActionParameters": null,
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": null,
    "ControllerActionDescriptorActionName": null
  }
}
```

#### 19

```json
{
  "HttpMethod": "GET",
  "Path": "/MinimalApi/123",
  "HttpRouteByRawText": null,
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": "",
  "DebugInfo": {
    "RawText": null,
    "RouteDiagnosticMetadata": null,
    "RouteData": {},
    "AttributeRouteInfo": null,
    "ActionParameters": null,
    "PageActionDescriptorRelativePath": null,
    "PageActionDescriptorViewEnginePath": null,
    "ControllerActionDescriptorControllerName": null,
    "ControllerActionDescriptorActionName": null
  }
}
```
