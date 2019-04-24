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

For the basic Validator, validations occur as that line of code executes.  For Generic Validators the expressions are stored and only executed when `.Validate()` is called.

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
 
validations can be used in a fluent fashion.

### Fluent Validator
 ```c#
 var validator = new Validator();
 var result = validator
	.isMinValue(someInt, 18)
	.IsEmailAddress(someVariable)
	.Validate();
 ```

For a fuller explaination see, [Details on how to use the validators](https://github.com/twistedtwig/AutoValidator/wiki/Validator-usage)



 [Further Reading](https://github.com/twistedtwig/AutoValidator/wiki)


### TODO 
 - [ ] add DI example code to https://github.com/twistedtwig/AutoValidator/wiki/Dependency-injection-suggestion for autofac func setup
 - [ ] have the ability to clear validation rules for T (only within that instance of a validator)
 - [ ] create examples for usage (error messages, standard and custom)
 - [ ] custom expression error message string format options
 - [ ] standard expression error message string format options
 - [ ] validating lists of simple types
 - [ ] test error message override in classValidator, regular string validator and regular fluent validator
 - [ ] add use mapping expression to the iClassValidator so that it will know to check for other mapping expressions to validate that object
 - [ ] validating single child property (add mappingexpression collection object, that will recursively get all child mappings)
 - [ ] validating child property lists
 - [ ] add more IValidatorExpression functions (have a look at https://github.com/gnpretorius/simple-validator)
 
