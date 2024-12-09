function exerciseForm(initialExercises) {
    return {
        exercises: initialExercises || [],

        updateValidation() {
            setTimeout(() => {
                $('#trainingForm')
                    .removeData("validator")
                    .removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse('#trainingForm');
            }, 0);
        },

        addExercise(type) {
            this.exercises.push({
                name: '',
                description: '',
                type: type,
                sets: type === 0
                    ? [{ repetitions: '', weight: '', exerciseType: type }]
                    : type === 1
                        ? [{ duration: '', distance: '', exerciseType: type }]
                        : [{ duration: '', exerciseType: type }]
            });
            this.updateValidation();
        },

        removeExercise(index) {
            this.exercises.splice(index, 1);
            this.updateValidation();
        },

        addSet(exercise) {
            if (exercise.type === 0) {
                exercise.sets.push({ repetitions: '', weight: '', exerciseType: exercise.type });
            } else if (exercise.type === 1) {
                exercise.sets.push({ duration: '', distance: '', exerciseType: exercise.type });
            } else if (exercise.type === 2) {
                exercise.sets.push({ duration: '', exerciseType: exercise.type });
            }
            this.updateValidation();
        }
    };
}

document.addEventListener("DOMContentLoaded", function () {
    $('#trainingForm').on('submit', function () {
        setTimeout(() => {
            $('#trainingForm')
                .removeData("validator")
                .removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse('#trainingForm');
        }, 0);
    });
});