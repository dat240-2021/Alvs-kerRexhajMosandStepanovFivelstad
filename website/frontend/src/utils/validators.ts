const isCharUpper = (char: string): boolean => char === char.toUpperCase();
const isCharLower = (char: string): boolean => char === char.toLowerCase();
const letterExpr = new RegExp("[a-zA-Z]");
const specialCharExpr = new RegExp("[0-9]");
const numericExpr = new RegExp("[*#+@!;.,$?%&/()=]");

const isEmpty = (value: string): boolean => !value.trim().length;
const isNotEmpty = (value: string): boolean => !isEmpty(value);

const hasMinLength =
  (len: number) =>
  (value: string): boolean =>
    value.length >= len;
const hasMaxLength =
  (len: number) =>
  (value: string): boolean =>
    value.length <= len;

const containsLetters = (value: string): boolean => letterExpr.test(value);
const containsNumbers = (value: string): boolean => numericExpr.test(value);
const containsSpecialChars = (value: string): boolean =>
  specialCharExpr.test(value);

const hasUpperCase = (value: string): boolean =>
  value.split("").some(isCharUpper);

const hasLowerCase = (value: string): boolean =>
  value.split("").some(isCharLower);

const areEqual = (value1: string, value2: string): boolean => value1 === value2;

const passwordValidators = [
  isNotEmpty,
  hasMinLength(8),
  containsLetters,
  containsNumbers,
  containsSpecialChars,
];

export default {
  isEmpty,
  isNotEmpty,
  hasMinLength,
  hasMaxLength,
  hasUpperCase,
  hasLowerCase,
  containsLetters,
  containsNumbers,
  containsSpecialChars,
  areEqual,
  passwordValidators,
};
