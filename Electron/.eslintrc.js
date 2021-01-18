module.exports = {
  extends: [
    "erb/typescript",
    "eslint:recommended",
    "plugin:react/recommended",
    "plugin:@typescript-eslint/eslint-recommended",
    "plugin:@typescript-eslint/recommended",
    "prettier/@typescript-eslint",
    "plugin:prettier/recommended",
  ],
  rules: {
    // A temporary hack related to IDE not resolving correct package.json
    "import/no-extraneous-dependencies": "off",
    "react/jsx-one-expression-per-line": "off",
    "react/destructuring-assignment": "off",
    "@typescript-eslint/no-empty-interface": "off",
    "react/no-access-state-in-setstate": "off",
    "no-restricted-syntax": "off",
    "no-plusplus": "off",
    "func-names": "off",
    "react/jsx-curly-newline": "off",
    "jsx-a11y/no-autofocus": "off",
    "no-restricted-syntax": "off",
    "no-else-return": "off",
    semi: [2, "never"],
    "react/jsx-filename-extension": [
      1,
      { extensions: [".js", ".jsx", ".ts", ".tsx"] },
    ],
    "import/extensions": [
      "error",
      "ignorePackages",
      {
        js: "never",
        jsx: "never",
        ts: "never",
        tsx: "never",
      },
    ],
    "react/prop-types": 0,
    "import/prefer-default-export": 0,
    "react/no-danger": 0,
  },
  plugins: ["react", "@typescript-eslint", "prettier"],
  parser: "@typescript-eslint/parser",
  parserOptions: {
    ecmaVersion: 2020,
    sourceType: "module",
    project: "./tsconfig.json",
    tsconfigRootDir: __dirname,
    createDefaultProgram: true,
    ecmaFeatures: {
      jsx: true, // Allows for the parsing of JSX
    },
  },
  settings: {
    "import/resolver": {
      // See https://github.com/benmosher/eslint-plugin-import/issues/1396#issuecomment-575727774 for line below
      node: {},
      webpack: {
        config: require.resolve("./internals/configs/webpack.config.eslint.js"),
      },
    },
    "import/parsers": {
      "@typescript-eslint/parser": [".ts", ".tsx"],
    },
  },
}
