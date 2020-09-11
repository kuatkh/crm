module.exports = {
  "parser": "babel-eslint",
  "env": {
        "browser": true,
        "commonjs": true,
        "es6": true,
        "jest/globals": true
  },
  "parserOptions": {
    "ecmaFeatures": {
      "experimentalObjectRestSpread": true,
      "jsx": true
    },
    "sourceType": "module"
  },
  "plugins": [
		"react",
    "flowtype",
    "jest"
  ],
  "rules": {
        "for-direction": "error",
        "no-await-in-loop": "error",
        "no-compare-neg-zero": "error",
        "no-cond-assign": "error",
        "no-constant-condition": "error",
        "no-control-regex": "error",
        "no-dupe-args": "error",
        "no-dupe-keys": "error",
        "no-duplicate-case": "error",
        "no-empty": "error",
        "no-empty-character-class": "error",
        "no-ex-assign": "error",
        "no-extra-boolean-cast": "error",
        "no-extra-parens": [
          "error",
          "all",
          {
            "ignoreJSX": "multi-line"
          }
        ],
        "no-extra-semi": "error",
        "no-func-assign": "error",
        "no-inner-declarations": "error",
        "no-invalid-regexp": "error",
        "no-irregular-whitespace": [
          "error",
          {
            "skipTemplates": true,
             "skipRegExps": true,
             "skipComments": true
           }
         ],
         "no-obj-calls": "error",
         "no-regex-spaces": "error",
         "no-sparse-arrays": "error",
         "no-template-curly-in-string": "error",
         "no-unexpected-multiline": "error",
         "no-unreachable": "error",
         "no-unsafe-finally": "error",
         "no-unsafe-negation": "error",
         "use-isnan": "error",
         "valid-typeof": "error",
        //  "array-callback-return": "error",
         "block-scoped-var": "error",
        //  "consistent-return": "error",
         "default-case": "error",
         "dot-notation": "error",
        //  "eqeqeq": "error",
        //  "no-alert": "error",
         "no-caller": "error",
         "no-div-regex": "error",
         "no-empty-pattern": "error",
        //  "no-eq-null": "error",
         "no-eval": "error",
         "no-extra-label": "error",
         "no-fallthrough": "error",
         "no-floating-decimal": "error",
         "no-global-assign": "error",
         "no-implicit-coercion": "error",
         "no-implicit-globals": "error",
         "no-implied-eval": "error",
        //  "no-invalid-this": "error",
         "no-labels": "error",
         "no-lone-blocks": "error",
         "no-loop-func": "error",
         "no-multi-spaces": "error",
         "no-multi-str": "error",
         "no-new": "error",
         "no-new-func": "error",
         "no-octal": "error",
         "no-octal-escape": "error",
        //  "no-param-reassign": "error",
         "no-proto": "error",
         "no-redeclare": "error",
        //  "no-return-assign": "error",
         "no-script-url": "error",
         "no-self-assign": "error",
         "no-self-compare": "error",
         "no-sequences": "error",
         "no-throw-literal": "error",
         "no-unmodified-loop-condition": "error",
        //  "no-unused-expressions": "error",
         "no-useless-call": "error",
         "no-useless-concat": "error",
         "no-useless-return": "error",
         "no-void": "error",
         "no-with": "error",
         "prefer-promise-reject-errors": "error",
         "radix": [
           "error",
           "as-needed"
         ],
         "require-await": "error",
         "no-catch-shadow": "error",
         "no-delete-var": "error",
         "no-shadow": "error",
         "no-shadow-restricted-names": "error",
         "no-undef": "error",
         "no-undef-init": "error",
        //  "no-undefined": "error",
         "no-unused-vars": "warn",
        //  "no-use-before-define": "error",
         "array-bracket-spacing": "error",
         "block-spacing": [
           "error",
           "always"
         ],
         "brace-style": [
           "error",
           "1tbs",
           {
             "allowSingleLine": true
           }
         ],
         "comma-dangle": [
           "error",
           "always-multiline"
         ],
         "computed-property-spacing": "error",
         "consistent-this": "error",
         "eol-last": "error",
         "func-call-spacing": "error",
         //"func-names": "error",
         //"func-style": "error",
         "indent": [
           "error",
           "tab",
					 {
						 "SwitchCase": 1,
						 "MemberExpression": 1
					 }
         ],
         "jsx-quotes": [
           "error",
           "prefer-single"
         ],
         "keyword-spacing": "error",
        //  "linebreak-style": [
        //    "error",
        //    "windows",
        //  ],
         "max-depth": "error",
         "max-nested-callbacks": [
           "error",
           {
             "max": 4,
           },
         ],
         "newline-per-chained-call": [
           "error",
           {
             "ignoreChainWithDepth": 4,
           }
         ],
         "no-array-constructor": "error",
         "no-bitwise": "error",
         "no-lonely-if": "error",
         "no-mixed-spaces-and-tabs": "error",
         "no-multi-assign": "error",
         "no-multiple-empty-lines": "warn",
         "no-new-object": "error",
         "no-trailing-spaces": "warn",
         "no-unneeded-ternary": "error",
         "no-whitespace-before-property": "error",
         "object-curly-spacing": "error",
         "object-property-newline": [
					 "error",
					 {
						 "allowMultiplePropertiesPerLine": true,
					 },
				 ],
         "operator-assignment": "error",
         "operator-linebreak": [
           "error",
           "before"
         ],
         "quotes": [
           "error",
           "single"
         ],
         "semi": [
           "error",
           "never",
         ],
         "space-before-blocks": "error",
         "arrow-body-style": [
           "warn",
           "as-needed",
         ],
         "arrow-parens": [
           "warn",
           "as-needed",
         ],
         "arrow-spacing": "error",
         "no-useless-constructor": "warn",
         "no-useless-computed-key": "warn",
         "no-this-before-super": "error",
         "constructor-super": "warn",
         "no-class-assign": "error",
         "no-const-assign": "error",
         "no-dupe-class-members": "error",
         "no-duplicate-imports": "error",
         "no-new-symbol": "error",
         "no-useless-rename": "error",
        //  "no-var": "error",
         "object-shorthand":  "warn",
        //  "prefer-const": "warn",
         "prefer-rest-params": "error",
         "prefer-spread": "warn",
         "require-yield": "error",
         "rest-spread-spacing": "error",
         "template-curly-spacing": "error",

        "flowtype/boolean-style": [
      2,
      "boolean"
    ],
    "flowtype/define-flow-type": 1,
    "flowtype/delimiter-dangle": [
      "warn",
      "always-multiline"
    ],
    "flowtype/generic-spacing": [
      2,
      "never"
    ],
    "flowtype/no-primitive-constructor-types": 2,
    "flowtype/no-types-missing-file-annotation": 1,
    "flowtype/object-type-delimiter": [
      2,
      "comma"
    ],
    "flowtype/require-valid-file-annotation": 2,
    "flowtype/semi": [
      "error",
      "never"
    ],
    "flowtype/space-after-type-colon": [
      2,
      "always"
    ],
    "flowtype/space-before-generic-bracket": [
      2,
      "never"
    ],
    "flowtype/space-before-type-colon": [
      2,
      "never"
    ],
    "flowtype/union-intersection-spacing": [
      2,
      "always"
    ],
    "flowtype/use-flow-type": 1,
    "flowtype/valid-syntax": 1,
		"react/jsx-uses-react": "error",
    "react/jsx-uses-vars": "error",
    "jest/no-disabled-tests": "warn",
    "jest/no-focused-tests": "error",
    "jest/no-identical-title": "error",
    "jest/valid-expect": "error"
  },
  "settings": {
    "flowtype": {
      "onlyFilesWithFlowAnnotation": true
    }
  }
};
