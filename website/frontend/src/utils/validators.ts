// const isCharLower = (char: string): boolean => char === char.toLowerCase();
const letterExpr = new RegExp("[a-z]");
const numericExpr = new RegExp("[0-9]");

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

const containsLetters = (value: string): boolean => letterExpr.test(value.toLowerCase());
const containsNumbers = (value: string): boolean => numericExpr.test(value);



const areEqual = (value1: string, value2: string): boolean => value1 === value2;

const passwordValidators = [
  isNotEmpty,
  hasMinLength(6),
  containsLetters,
  containsNumbers,
];

export default {
  isEmpty,
  isNotEmpty,
  hasMinLength,
  hasMaxLength,
  containsLetters,
  containsNumbers,
  areEqual,
  passwordValidators,
};
