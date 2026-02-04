# Asset Bundling Pattern (Surpath112)

[2026-02-02|62b86a8]

## Surpath112 Only (MVC + jQuery)

In surpath112, JavaScript and CSS are bundled dynamically via Gulp.

## How It Works

### 1. Add JavaScript File

Create JS file in proper location:
```
wwwroot/view-resources/Areas/App/Views/{Controller}/{File}.js
```

### 2. Reference in bundles.json

Edit `bundles.json` to include the file:
```json
{
  "scripts": [
    {
      "output": "view-resources/Areas/App/Views/MyController/_MyPage.min.js",
      "input": [
        "wwwroot/view-resources/Areas/App/Views/MyController/_MyPage.js"
      ]
    }
  ]
}
```

### 3. Run Gulp

Development (with watch):
```bash
cd src/inzibackend.Web.Mvc
gulp buildDev
```

Production (minified):
```bash
gulp build
```

### 4. Reference in Razor View

```html
@section Scripts {
    <script src="~/view-resources/Areas/App/Views/MyController/_MyPage.min.js" asp-append-version="true"></script>
}
```

## Critical Rules

**DO**:
- Give HTML elements unique, descriptive class names
- Reference elements by class in JavaScript
- Use `.on()` for event binding
- Keep JS files organized by view

**DON'T**:
- Manually minify files (Gulp handles it)
- Reference non-bundled files in production
- Use ID selectors (class selectors preferred)
- Put business logic in JS (use service proxies)

## Migration Note

**Surpath200 (React SPA)**: This pattern is NOT used. Vite handles bundling automatically.

## Related

- (see: jquery-to-react/asset-migration) How bundles convert to React imports
- Source: `CLAUDE.md:152-156`
- Source: `surpath112/.cursorrules:23-25`
