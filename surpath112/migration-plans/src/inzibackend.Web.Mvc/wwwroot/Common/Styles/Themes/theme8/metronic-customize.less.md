# Modified
## Filename
metronic-customize.less
## Relative Path
inzibackend.Web.Mvc\wwwroot\Common\Styles\Themes\theme8\metronic-customize.less
## Language
Unknown
## Summary
Both modified and unmodified files import similar CSS modules but differ in their styling of select2 dropdowns. The modified file adds more specific styling for .select2-dropdown and its input.
## Changes
.select2-dropdown {    .select2-dropdown--below{        background-color: #1e1e2d !important;        border: 1px solid #323248 !important;        color: #999 !important;        input{            background-color: #1e1e2d !important;            border: 1px solid #323248 !important;            color: #999 !important;            &:focus-visible{                border: 1px solid #323248 !important;            }        }    }}.select2-selection {    background-color: rgb(18, 18, 30) !important;    border: 1px solid rgb(44, 45, 68) !important;    color: #9899ac !important;}.select2-selection:focus {    border-color: #7380fc !important;    color: #fff !important;}.searchMenuSelect2 {    background-color: rgb(18, 18, 30) !important;    border: 1px solid rgb(44, 45, 68) !important;    color: #9899ac !important;}.searchMenuSelect2:focus {    border-color: #7380fc !important;    color: #fff !important;}
## Purpose
Styling select2 dropdowns in dark mode themes
