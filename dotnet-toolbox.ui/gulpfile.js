var gulp = require('gulp');
var connect = require('gulp-connect');

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

gulp.task('default', ['webserver']);
