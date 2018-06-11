"use strict";


(function (WorkoutsCreate, $, undefined) {
    // Handles functionality for /workouts/create view.
    // Uses some newer ES6 features, avoiding jQuery for demonstration
    // except where using Bootstrap functionality.


    var self = WorkoutsCreate;


    var config = {
        classes: {
            exerciseInstance: 'exercise-instance',
            exerciseSet: 'exercise-set',
            triggerAddExercise: 'workout__trigger--add-exercise'
        },
        sessionStorageKeys: {
            addExerciseTriggeringNodeId: 'workoutsCreate__add-exercise__triggering-node-id'
        }
    }


    self.init = function () {
        // Prevent any confusion over scope of 'this' in functions.
        document.addEventListener('DOMContentLoaded', function (event) {
            console.debug('DOM content loaded.');

            self.validateDependencies();
            self.bindEvents();
            self.setWorkoutDateToCurrentDateTime();

            console.debug('Initialized workoutCreate.');
        });
    }


    self.validateDependencies = function () {
        // Ensure browser supports <template> element.
        if (!'content' in document.createElement('template')) {
            throw 'This application requires a browser with template support, such as Chrome, to run.';
        }

        // Ensure browser supports session & local storage.
        if (typeof (Storage) === "undefined") {
            throw 'This application requires a browser with session storage.';
        }
    }


    self.bindEvents = function () {
        console.debug('Binding events...');

        self.bindExerciseSelectionModal();
        self.bindExerciseInstanceAddTrigger();
        self.bindExerciseInstanceRemoveTrigger();
        self.bindExerciseSetAddTrigger();
        self.bindExerciseSetRemoveTrigger();

        console.debug('Finished binding events.');
    }


    self.addExerciseToWorkout = function (exerciseId, exerciseName, triggeringNodeId) {
        console.debug(`Adding exercise with id [${exerciseId}], name [${exerciseName}], and triggering node id [${triggeringNodeId}].`);

        let exerciseInstance = self
            .getTemplateContentFor(config.classes.exerciseInstance)
            .querySelector(`.${config.classes.exerciseInstance}`);

        let namePrefix = `ExerciseInstances[${self.getExerciseInstanceCount()}]`;

        self.replaceProperties(exerciseInstance, '#ExerciseId', namePrefix, '.ExerciseId', ['id', 'name']);
        self.replaceProperties(exerciseInstance, '#ExerciseName', namePrefix, '.ExerciseName', ['id', 'name']);
        self.replaceProperties(exerciseInstance, `[name="${namePrefix}.ExerciseName"]`, '', exerciseName, ['value']);
        self.replaceProperties(exerciseInstance, `.${config.classes.exerciseInstance}__name`, '', exerciseName, ['innerHTML']);

        // NOT the same as the exercise ID. This ID lets us uniquely find the exercise in the document (there may be more than one instance of the same exercise).
        let uniqueExerciseInstanceId = self.createUniqueId();
        self.replaceProperties(exerciseInstance, `[name="${namePrefix}.ExerciseId"]`, '', uniqueExerciseInstanceId, ['value'])

        let triggeringNode = document.getElementById(triggeringNodeId);
        triggeringNode.parentNode.insertBefore(exerciseInstance, triggeringNode);

        exerciseInstance.id = uniqueExerciseInstanceId;
        self.addExerciseSetToExercise(uniqueExerciseInstanceId);
    }


    self.addExerciseSetToExercise = function (exerciseInstanceId) {
        console.debug(`Adding set to exercise instance with id [${exerciseInstanceId}].`);

        let exerciseInstance = document.getElementById(exerciseInstanceId);
        let exerciseInstanceSets = exerciseInstance.getElementsByClassName(`${config.classes.exerciseSet}__container`);

        // Modify values for these fields so they POST correctly as arrays,
        // e.g ExerciseInstances[2].ExerciseSet[1].id
        let namePrefix = `ExerciseInstances[${self.getExerciseInstanceCount() - 1}].ExerciseSets[${exerciseInstanceSets.length}]`;

        let content = self.getTemplateContentFor(`${config.classes.exerciseSet}-instance`);

        self.replaceProperties(content, `.${config.classes.exerciseSet}__field--repetitions`, namePrefix, '.Repetitions', ['id', 'name']);
        self.replaceProperties(content, `.${config.classes.exerciseSet}__field--weight`, namePrefix, '.Weight', ['id', 'name']);

        let repetitionsFieldValidator = content.querySelector(`[data-valmsg-for="Repetitions"]`);;
        repetitionsFieldValidator.setAttribute('data-valmsg-for', `${namePrefix}.Repetitions`);

        let weightFieldValidator = content.querySelector(`[data-valmsg-for="Weight"]`);
        weightFieldValidator.setAttribute('data-valmsg-for', `${namePrefix}.Weight`);

        // Add to exercise instance.
        exerciseInstance
            .getElementsByClassName(`${config.classes.exerciseInstance}__${config.classes.exerciseSet}-list`)[0]
            .appendChild(content);

        // Without this, unobtrusive validation will ignore the newly added element.
        self.reparseFormValidation();
    }


    self.bindExerciseSelectionModal = function () {
        console.debug('Binding exercise selection modal.');

        $(self.getExerciseModal()).on('shown.bs.modal', function (e) {
            /* 
             Use jQuery here since a dev familiar with bootstrap would
             be most familiar with the bootstrap/jquery way of managing bootstrap 
             components (bootstrap and examples use jQuery) 
            */
            var $modal = $(this);

            var $triggeringElement = $(e.relatedTarget);
            sessionStorage.setItem(
                config.sessionStorageKeys.addExerciseTriggeringNodeId,
                e.relatedTarget.id
            );

            var $modalBody = $modal.find('.modal-body');

            var $modalLoadingIndicator = $modalBody.find('.exercise-modal__loading');
            $modalLoadingIndicator.show();

            var $exercisesList = $(self.getExerciseModalList());
            $exercisesList.empty();

            /*
             Get list of exercises from the server. 
             Retrieve new list each time since user can       
             choose exercises from exercises created by other 
             users, they may have added new exercises since 
             last time modal was opened.
            */
            $.get('/api/exercises', function (response) {
                $modalLoadingIndicator.hide();
                $exercisesList.empty();

                // Add the exercises retrieved from the server to the modal body.
                response
                    .sort(function (a, b) {
                        return ('' + a.name).localeCompare(b.name);
                    })
                    .forEach(function (exercise) {
                        $exercisesList.append(
                            `<a href="#" data-exercise-id="${exercise.id}" 
                                class="${config.classes.triggerAddExercise} list-group-item list-group-item-action" data-dismiss="modal">
                                ${exercise.name}
                            </a>`);
                    });
            });
        });
    }


    self.bindExerciseInstanceAddTrigger = function () {
        console.debug('Binding exercise add trigger.');

        self
            .getExerciseModalList()
            .addEventListener('click', function (e) {
                console.debug('Exercises modal list click.');

                let target = e.target;

                if (target.classList.contains(config.classes.triggerAddExercise)) {
                    self.addExerciseToWorkout(
                        target.dataset.exerciseId,
                        target.innerHTML,
                        sessionStorage.getItem(config.sessionStorageKeys.addExerciseTriggeringNodeId)
                    );
                }
            });
    }


    self.bindExerciseInstanceRemoveTrigger = function () {
        self
            .getExerciseContainer()
            .addEventListener('click', function (e) {
                console.debug('Exercises container click.');

                let removeTriggerSelector = `${config.classes.exerciseInstance}__trigger--remove-${config.classes.exerciseInstance}`;

                if (e.target.classList.contains(removeTriggerSelector) ||
                    e.target.parentNode.classList.contains(removeTriggerSelector)) {
                    console.debug('Exercise instance remove trigger clicked.');

                    e.preventDefault();
                    if (!confirm('Are you sure you want to delete this exercise?')) return;

                    let exerciseSetContainerSelector = `.${config.classes.exerciseInstance}`;
                    console.debug(`Remove exercise set trigger clicked for ${exerciseSetContainerSelector}.`);

                    e.target.closest(exerciseSetContainerSelector).remove();
                }
            });
    }


    self.bindExerciseSetAddTrigger = function () {
        console.debug('Binding exercise set add trigger.');

        self
            .getExerciseContainer()
            .addEventListener('click', function (e) {
                console.debug('Exercises container click.');

                if (e.target.classList.contains(`${config.classes.exerciseInstance}__trigger--add-${config.classes.exerciseSet}`)) {
                    console.debug('Add exercise set trigger click.');
                    e.preventDefault();

                    let exerciseInstanceId = e.target.closest(`.${config.classes.exerciseInstance}`).id;
                    self.addExerciseSetToExercise(exerciseInstanceId);
                }
            });
    }


    self.bindExerciseSetRemoveTrigger = function () {
        console.debug('Bindinge exercise set remove trigger.');

        self
            .getExerciseContainer()
            .addEventListener('click', function (e) {
                console.debug('Exercises container click.');

                let removeTriggerSelector = `${config.classes.exerciseSet}__trigger--remove-${config.classes.exerciseSet}`;

                if (e.target.classList.contains(removeTriggerSelector) ||
                    e.target.parentNode.classList.contains(removeTriggerSelector)) {

                    console.debug('Exercise set remove trigger clicked.');

                    e.preventDefault();
                    if (!confirm('Are you sure you want to delete this exercise set?')) return;

                    let exerciseSetContainerSelector = `.${config.classes.exerciseSet}__container`;
                    console.debug(`Remove exercise set trigger clicked for ${exerciseSetContainerSelector}.`);

                    e.target.closest(exerciseSetContainerSelector).remove();
                }
            });
    }


    self.createUniqueId = function () {
        // Generates ID unique enough for this application.
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        );
    }


    self.getExerciseContainer = function () {
        return document.getElementById('excercise__container');
    }


    self.getExerciseInstanceCount = function () {
        /// <summary>Returns the count of exercise instances on the current page.</summary>  
        /// <returns type="Number">The count of exercise instances on the current page.</returns>
        return document.getElementsByClassName(config.classes.exerciseInstance).length;
    }


    self.getExerciseModal = function () {
        return document.getElementById('exercise-modal');
    }


    self.getExerciseModalList = function () {
        return self
            .getExerciseModal()
            .getElementsByClassName('exercise-modal__list')[0];
    }


    self.getTemplateContentFor = function (componentName) {
        let templateId = `workouts__templates__${componentName}`;
        console.debug(`Getting template for [${templateId}].`);

        let template = document.getElementById(templateId);

        return document.importNode(template.content, true);
    }


    self.reparseFormValidation = function () {
        // Use jQuery here (since it's jQuery validation) to re-parse the form, use after adding fields.
        let $form = $('#workouts-create__form');

        $form.removeData('validator').removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse($form);

        console.debug('Reparsed validation.');
    }


    self.replaceProperties = function (containingNode, fieldSelector, valuePrefix, value, propertiesToReplace) {
        /// <summary>
        /// Replaces the name and id attributes of the element with the given selector with the given
        /// value prefix concatenated with the given value.
        /// </summary>
        /// <param name="fieldSelector" type="String">The selector of the field, e.g [name="foo"].</param>
        /// <param name="valuePrefix" type="String">
        /// The prefix of the value to concatenate, e.g Foo[1].Bar[0].
        /// </param>
        /// <param name="value" type="String">
        /// The value to concatenate to the given value prefix, would usually be the field's original
        /// name e.g "Foo".
        /// </param>
        /// <returns type="Number">The area.</returns>

        let field = containingNode.querySelector(fieldSelector);
        let newValue = valuePrefix + value;

        console.debug(`Updating [${fieldSelector}] properties with value [${newValue}]...`);

        propertiesToReplace.forEach(function (propertyName) {
            console.debug(`Updating [${fieldSelector}] property [${propertyName}] with value [${newValue}]...`);
            field[propertyName] = newValue;
        });
    }


    self.setWorkoutDateToCurrentDateTime = function () {
        // Set workout date to now.
        Date.prototype.toDateTimeInputValue = (function () {
            var local = new Date(this);
            local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
            return local.toJSON();
        });

        document.getElementById('CreatedTimestamp').value = new Date().toDateTimeInputValue().slice(0, -8);
    }


}(window.WorkoutsCreate = window.WorkoutsCreate || {}, jQuery));