var gulp = require('gulp');
var connect = require('gulp-connect');
var jasmineBrowser = require('gulp-jasmine-browser');
var webpack = require('webpack-stream');
var plumber = require('gulp-plumber');
require('babel-loader');

gulp.task('webserver', function () {
    connect.server();
});

gulp.task('build', function () {
    return gulp.src('*.html')
        .pipe(gulp.dest('dist/'));
});

gulp.task('prepush', ['build'], function () {
    return gulp.src('dist/**/*')
        .pipe(gulp.dest('../dotnet-toolbox.api/wwwroot'));
});

gulp.task('jasmine', function () {
    var JasminePlugin = require('gulp-jasmine-browser/webpack/jasmine-plugin');
    var plugin = new JasminePlugin();
    return gulp.src(['src/**/*js', 'spec/**/*spec.js'])
        .pipe(plumber())
        .pipe(webpack({
            watch: true,
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
        }))
        .pipe(jasmineBrowser.specRunner())
        .pipe(jasmineBrowser.server());
});

gulp.task('default', ['webserver']);
