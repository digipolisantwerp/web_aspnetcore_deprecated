var gulp = require('gulp');
var toc = require('gulp-doctoc');

gulp.task('readme', function () {
    gulp.src("./README.md")
        .pipe(toc())
        .pipe(gulp.dest("."));
        
});

gulp.task('default', ['readme']);