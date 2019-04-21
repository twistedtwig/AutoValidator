<a href="http://home.houseofhawkins.com:8080/viewType.html?buildTypeId=AutoValidator_Nuget&guest=1">
<img src="http://home.houseofhawkins.com:8080/app/rest/builds/buildType:(id:AutoValidator_Nuget)/statusIcon"/>
</a>

# AutoValidator

## GOAL

AutoValidator's aim is to create a simple, fluent and intiutive framework to validate data and models in your dotnet applications. Simply put; an application would validate some data and get a result object.  That object would state sucess or failure and detail the failing data.

There are two basic ways to validate data:

 * Simply call validate on each item of data as you need to.
 * Define a validation schema for a model, then call validate on an instance of that model.

One of the principles AutoValidator follows is that exceptions should only happen in exceptional situations, so it will not throw an exception when some data is incorrect, it will simply return a result object detailing what is wrong.

the validation schema process is heavily guided by [AutoMapper](https://github.com/AutoMapper/AutoMapper).


## Getting Started

If you just want to validate individual variables, simply create an instance of `Validator`.

 ```c#
 var validator = new Validator();
 ```

If you wish to use schema validation; first create an instance of `MapperConfiguration`, define the `MapperConfigurationExpression` then create a factory to create instance of validators.

```c#
var mapper = new MapperConfiguration();
Action<IMapperConfigurationExpression> configure = cfg =>
{
    cfg.AddProfile<ModelToBeValidatedProfile>();
};

mapper = new MapperConfiguration(configure);

var factory =  mapper.CreateFactory();

var validator = factory.Create<ModelToBeValidated>();
```

You only need to create one instance of the mapper, its configuration and the factory.  Then use the factory instance to create new instances of the validator as required.

For further information about [configuration and setup](https://github.com/twistedtwig/AutoValidator/wiki/Mapper-Configuration-Setup)


## Basics of using validators

There are two types of validators.  Generic and non generic.

 - Generic validators use the schema validation process set out in [configuration and expression setup](https://github.com/twistedtwig/AutoValidator/wiki/Mapper-Configuration-Setup)
 - Non Generic validators are for validating individual variables.

 ### Generic Validator
 ```c#
var validator = factory.Create<ModelToBeValidated>();

var model = new ModelToBeValidated();

var result = validator.Validate(model);
 ```

 ### Non Generic Validator
 ```c#
 var validator = new Validator();
 var result = validator.IsEmailAddress(someVariable).Validate();
 ```
 
validations can be used in a fluent fashion.  Only when `Validate()` is called are they checked and the validation result returned.

### Fluent Validator
 ```c#
 var validator = new Validator();
 var result = validator
				.isMinValue(someInt, 18)
				.IsEmailAddress(someVariable)
				.Validate();
 ```

For a fuller explaination see, [Details on how to use the validators](https://github.com/twistedtwig/AutoValidator/wiki/Validator-usage)



 ## Further Reading

 - [Mapper Configuration and Mapper Configuration Expressions](https://github.com/twistedtwig/AutoValidator/wiki/Mapper-Configuration-Setup)
 - [Example of how you could use AutoValidator in a DI framework](https://github.com/twistedtwig/AutoValidator/wiki/Dependency-injection-suggestion)
 - [Schema Validation](https://github.com/twistedtwig/AutoValidator/wiki/Validation-Schemas)
 - [Asserting Schema Valiadtion](https://github.com/twistedtwig/AutoValidator/wiki/Asserting-Schema-validation)
 - [Validators](https://github.com/twistedtwig/AutoValidator/wiki/Validator-usage)
 - [Validation Result Object](https://github.com/twistedtwig/AutoValidator/wiki/Validator-Results)


### TODO
 - [x] fix up Teamcity
 - [x] add unit tests to Teamcity
 - [x] write unit tests for all IValidatorExpressions
 - [x] class validation and class expression validation assertion with error messages
 - [x] validation error messages need to be connected to the property name
 - [x] ValidationResult needs to be able to have multiple errors for each property, should have an array of errors as well as a nicely formatted string represntation.
 - [ ] be able to create expression and validate inline (without classValidator)
 - [x] classValidator creation finds mapping and adds expressions
 - [x] create configuration object for validation schema
 - [x] configuration setup to allow add mappings from assemblies
 - [x] creating custom validation function
 - [ ] allow global CreateMap to be defined in initial setup configuration
 - [x] ensure all mappings are being saved
 - [x] factory for class type to get instance of a validator with the defined mappings
 - [ ] be able to use the mapping style in simple validation process
 - [ ] have the ability to clear validation rules for T (only within that instance of a validator)
 - [ ] custom expression error message string format options
 - [ ] standard expression error message string format options
 - [ ] test error message override in classValidator, regular string validator and regular fluent validator
 - [x] create basic start up guide at beginning of readme
 - [ ] create wiki pages for more complex stuff and link to each from readme and home of wiki
 - [ ] add more IValidatorExpression functions
 - [ ] add DI example code to https://github.com/twistedtwig/AutoValidator/wiki/Dependency-injection-suggestion for autofac func setup
 - [ ] create examples for usage

