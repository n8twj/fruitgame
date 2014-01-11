set UNITY_EXE="C:\Program Files\Unity\Editor\Unity.exe"

mkdir build

%UNITY_EXE% -quit -batchmode -buildWebPlayer build/web .
if %errorlevel% neq 0 exit /b %errorlevel%
cp build/web/web.html build/web/index.html

REM publish the build to http://erosson.github.io/fruitgame/
REM https://help.github.com/articles/creating-project-pages-manually
rm -rf build/web_push
git clone . build/web_push
cd build/web_push
git checkout gh-pages
if %errorlevel% neq 0 exit /b %errorlevel%

rm -rf *
cp -rp ../web/* .
if %errorlevel% neq 0 exit /b %errorlevel%
git add *
if %errorlevel% neq 0 exit /b %errorlevel%
git commit -m "gh-pages updated by build script"
if %errorlevel% neq 0 exit /b %errorlevel%
git push
if %errorlevel% neq 0 exit /b %errorlevel%
cd ../..
git push origin gh-pages