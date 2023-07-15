| | Scenario | Expected | Actual |
| - | - | - | - |
| [1](#1) | ConventionalRouting | GET  | / |
| [2](#2) | ConventionalRouting | GET  | /ConventionalRoute/ActionWithStringParameter/2 |
| [3](#3) | ConventionalRouting | GET  | /ConventionalRoute/ActionWithStringParameter |
| [4](#4) | ConventionalRouting | GET  | /ConventionalRoute/NotFound |
| [5](#5) | AttributeRouting | GET  | AttributeRoute |
| [6](#6) | AttributeRouting | GET  | AttributeRoute/Get |
| [7](#7) | AttributeRouting | GET  | AttributeRoute/Get/{id} |
| [8](#8) | AttributeRouting | GET  | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate |
| [9](#9) | AttributeRouting | GET  | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate |
| [10](#10) | RazorPages | GET  | / |
| [11](#11) | RazorPages | GET  | Index |
| [12](#12) | RazorPages | GET  | PageThatThrowsException |
| [13](#13) | RazorPages | GET  | /js/site.js |

#### 1

```json
{
  "Path": "/",
  "RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "RouteDiagnosticMetadata": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "RouteData": "controller=ConventionalRoute,action=Default",
  "AttributeRouteInfo": null,
  "ActionParameters": "",
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": "ConventionalRoute",
  "ControllerActionDescriptorActionName": "Default"
}
```

#### 2

```json
{
  "Path": "/ConventionalRoute/ActionWithStringParameter/2??num=3",
  "RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "RouteDiagnosticMetadata": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "RouteData": "controller=ConventionalRoute,action=ActionWithStringParameter,id=2",
  "AttributeRouteInfo": null,
  "ActionParameters": "{id}/{num}",
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": "ConventionalRoute",
  "ControllerActionDescriptorActionName": "ActionWithStringParameter"
}
```

#### 3

```json
{
  "Path": "/ConventionalRoute/ActionWithStringParameter??num=3",
  "RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "RouteDiagnosticMetadata": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "RouteData": "controller=ConventionalRoute,action=ActionWithStringParameter",
  "AttributeRouteInfo": null,
  "ActionParameters": "{id}/{num}",
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": "ConventionalRoute",
  "ControllerActionDescriptorActionName": "ActionWithStringParameter"
}
```

#### 4

```json
{
  "Path": "/ConventionalRoute/NotFound",
  "RawText": null,
  "RouteDiagnosticMetadata": null,
  "RouteData": "",
  "AttributeRouteInfo": null,
  "ActionParameters": null,
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": null,
  "ControllerActionDescriptorActionName": null
}
```

#### 5

```json
{
  "Path": "/AttributeRoute",
  "RawText": "AttributeRoute",
  "RouteDiagnosticMetadata": "AttributeRoute",
  "RouteData": "action=Get,controller=AttributeRoute",
  "AttributeRouteInfo": "AttributeRoute",
  "ActionParameters": "",
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": "AttributeRoute",
  "ControllerActionDescriptorActionName": "Get"
}
```

#### 6

```json
{
  "Path": "/AttributeRoute/Get",
  "RawText": "AttributeRoute/Get",
  "RouteDiagnosticMetadata": "AttributeRoute/Get",
  "RouteData": "action=Get,controller=AttributeRoute",
  "AttributeRouteInfo": "AttributeRoute/Get",
  "ActionParameters": "",
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": "AttributeRoute",
  "ControllerActionDescriptorActionName": "Get"
}
```

#### 7

```json
{
  "Path": "/AttributeRoute/Get/12",
  "RawText": "AttributeRoute/Get/{id}",
  "RouteDiagnosticMetadata": "AttributeRoute/Get/{id}",
  "RouteData": "action=Get,controller=AttributeRoute,id=12",
  "AttributeRouteInfo": "AttributeRoute/Get/{id}",
  "ActionParameters": "{id}",
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": "AttributeRoute",
  "ControllerActionDescriptorActionName": "Get"
}
```

#### 8

```json
{
  "Path": "/AttributeRoute/12/GetWithActionNameInDifferentSpotInTemplate",
  "RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteDiagnosticMetadata": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteData": "action=GetWithActionNameInDifferentSpotInTemplate,controller=AttributeRoute,id=12",
  "AttributeRouteInfo": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "ActionParameters": "{id}",
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": "AttributeRoute",
  "ControllerActionDescriptorActionName": "GetWithActionNameInDifferentSpotInTemplate"
}
```

#### 9

```json
{
  "Path": "/AttributeRoute/NotAnInt/GetWithActionNameInDifferentSpotInTemplate",
  "RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteDiagnosticMetadata": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteData": "action=GetWithActionNameInDifferentSpotInTemplate,controller=AttributeRoute,id=NotAnInt",
  "AttributeRouteInfo": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "ActionParameters": "{id}",
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": "AttributeRoute",
  "ControllerActionDescriptorActionName": "GetWithActionNameInDifferentSpotInTemplate"
}
```

#### 10

```json
{
  "Path": "/",
  "RawText": "",
  "RouteDiagnosticMetadata": "",
  "RouteData": "page=/Index",
  "AttributeRouteInfo": "",
  "ActionParameters": "",
  "PageActionDescriptorRelativePath": "/Pages/Index.cshtml",
  "PageActionDescriptorViewEnginePath": "/Index",
  "ControllerActionDescriptorControllerName": null,
  "ControllerActionDescriptorActionName": null
}
```

#### 11

```json
{
  "Path": "/Index",
  "RawText": "Index",
  "RouteDiagnosticMetadata": "Index",
  "RouteData": "page=/Index",
  "AttributeRouteInfo": "Index",
  "ActionParameters": "",
  "PageActionDescriptorRelativePath": "/Pages/Index.cshtml",
  "PageActionDescriptorViewEnginePath": "/Index",
  "ControllerActionDescriptorControllerName": null,
  "ControllerActionDescriptorActionName": null
}
```

#### 12

```json
{
  "Path": "/PageThatThrowsException",
  "RawText": "PageThatThrowsException",
  "RouteDiagnosticMetadata": "PageThatThrowsException",
  "RouteData": "page=/PageThatThrowsException",
  "AttributeRouteInfo": "PageThatThrowsException",
  "ActionParameters": "",
  "PageActionDescriptorRelativePath": "/Pages/PageThatThrowsException.cshtml",
  "PageActionDescriptorViewEnginePath": "/PageThatThrowsException",
  "ControllerActionDescriptorControllerName": null,
  "ControllerActionDescriptorActionName": null
}
```

#### 13

```json
{
  "Path": "/js/site.js",
  "RawText": null,
  "RouteDiagnosticMetadata": null,
  "RouteData": "",
  "AttributeRouteInfo": null,
  "ActionParameters": null,
  "PageActionDescriptorRelativePath": null,
  "PageActionDescriptorViewEnginePath": null,
  "ControllerActionDescriptorControllerName": null,
  "ControllerActionDescriptorActionName": null
}
```
