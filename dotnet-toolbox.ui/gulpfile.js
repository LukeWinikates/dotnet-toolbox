var gulp = require('gulp');
var connect = require('gulp-connect');
var jasmineBrowser = require('gulp-jasmine-browser');
var webpack = require('webpack-stream');
var plumber = require('gulp-plumber');
var jade = require('gulp-jade');
var sass = require('gulp-sass');
var rename = require('gulp-rename');
var replace = require('gulp-replace');

require('babel-loader');
var JasminePlugin = require('gulp-jasmine-browser/webpack/jasmine-plugin');

var paths = {
  jade: 'src/html/**/*.jade',
  sass: 'src/styles/**/*.scss',
  fonts: 'bower_components/pivotal-ui/src/pivotal-ui/components/typography/fonts/**/*',
  dotnetWWWRoot: '../src/dotnet-toolbox.api/wwwroot'
};

gulp.task('build', ['sass', 'fonts', 'jade']);

gulp.task('fonts', function(){
  gulp.src(paths.fonts)
    .pipe(gulp.dest('dist/styles/fonts'));
});

gulp.task('webserver', ['build', 'watch'], function () {
  connect.server({root: 'dist'});
});

gulp.task('jade', function () {
  return gulp.src(paths.jade)
    .pipe(jade())
    .pipe(rename(function (path) {
      path.extname = '.html'
    }))
    .pipe(gulp.dest('dist/'));
});

gulp.task('sass', function () {
  return gulp.src(paths.sass)
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest('./dist/styles'));
});

gulp.task('watch', function () {
  gulp.watch(paths.jade, ['jade']);
  gulp.watch(paths.sass, ['sass']);
});

gulp.task('prepush', ['build'], function () {
  return gulp.src('dist/**/*')
    .pipe(gulp.dest(paths.dotnetWWWRoot));
});

function sharedJasmineSetup(opts) {
  var opts = opts || {};
  var plugin = new JasminePlugin();
  return gulp.src(['src/**/*js', 'spec/**/*spec.js'])
    .pipe(plumber())
    .pipe(webpack({
      watch: !!opts.watch,
      module: {
        loaders: [
          {
            test: /\.js$/,
            exclude: /node_modules/,
            loader: 'babel-loader?stage=0&optional[]=runtime&loose=true'
          }
        ]
      }, output: {filename: 'spec.js'},
      plugins: [plugin]
    }));
}

gulp.task('jasmine', function () {
  return sharedJasmineSetup({watch: true})
    .pipe(jasmineBrowser.specRunner())
    .pipe(jasmineBrowser.server());
});

gulp.task('jasmine-phantom', function () {
  return sharedJasmineSetup()
    .pipe(jasmineBrowser.specRunner({console: true}))
    .pipe(jasmineBrowser.headless());
});

gulp.task('default', ['webserver']);
