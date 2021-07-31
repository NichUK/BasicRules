# BasicRules
Basic rules engine. Uses Autofac as rules repository and strongly-typed rules

This rules engine is designed to be very simple and basic. It might work for your requirement, or it might not. 
Other rules engines are definitely more mature than this one, probably faster, and definitely have more features. 
But it you're looking for a basic loosely inference-based rules engine that supports forward-chaining and strongly-typed rules, then this might fit the bill.

Currently only strongly typed, coded rules are allowed, although since they are simple classes with expressions for logic, it wouldn't be hard to implement a parser for a storage medium. All rules inherit from the Rule abstract baseclass.

Rules (and everything else) are stored in Autofac, and a new lifetime scope is created at each layer.

**Layers**
  * Workspace: Designed to be long-lasting - life of application. Rules are loaded into this.
  * Session: Designed to be created as required to execute a rule cycle. Rule are instanciated in this.
  * RuleEngine: Created automatically within a session for rule execution. Rules are matched for execution, and executed in this.

**Rule Lifecycle**
  * Workspace: Rules types are registered into Autofac.
  * Session: Rules are instantiated and stored ready for matching.
  * Rule Engine: (standard running)
    * Match: Matches rules with available facts (data) from Autofac or Session.Input. Rules which match are Activated.
           Data matching is perfomed at the same time. If no data matches, then the rule does not match, and is not activated.
    * Resolve: When multiple rules are activated in a single cycle, Resolve method is used to rank them. Highest rank is executed.

    * Rule: (The chosen rule)
        * PreAct: An optional set-up phase
        * Act: The chosen rule is executed. If data is an IEnumerable, the rule is run on all data items.
        * PostAct: An options post-Act phase. Can be used to store output data.
        * The rule is then marked as Fired.
  
The cycle is then repeated until no rules match.

**Options**
  * Rule
    * FireMultiple: A rule can override FireMultiple and set it true. This allows a rule to be fired multiple times within a session. e.g. for an iterative process.
      If FireMultiple is set true, then the HaltFunc parameter must be supplied in the GetDataMatches function to tell the engine when to halt the cycle.
    
