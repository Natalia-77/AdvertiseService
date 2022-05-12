APi Description/

This application has only API example.

POST gets a model which consider:
-title
-description
-price
-date time(default value-DateTime.UtcNow)
-array of images(gets base 64 image)

Model fields must be valid.Its used FluentValidation package.
Image convert into picture and saved into folder project "images".
One of them:
https://advertise.ml/images/yo2gfxg3.hur.jpg

GET list of items request has pagination option.Response returns 10 position of items,number of current page,
number of last page,total count of items in database.
If user entered incorrect number of page-he redidrected on the first page of list.

For handle exceptions into app I used custom class ExceptionHandler.

API:https://advertise.ml/swagger/index.html
